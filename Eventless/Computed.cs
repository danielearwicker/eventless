using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Threading;

namespace Eventless
{
    public class Computed<T> : SetableImpl<T>, IGetable<T>, ICanThrottle<Computed<T>>, IDisposable
    {
        private readonly Func<T> _compute;

        private ISet<INotifyPropertyChanged> _subscriptions = Computed.EmptySubscriptions;

        private Action _throttledRecompute;

        private int _listenerCount;

        public Computed(Func<T> compute)
            : base(new Setable<T>())
        {
            _compute = compute;
            _throttledRecompute = Recompute;
        }

        public Computed<T> SetThrottler(Func<Action, Action> throttler)
        {
            _throttledRecompute = throttler(Recompute);
            return this;
        }

        private void RecomputeSoon(object sender, PropertyChangedEventArgs args)
        {
            _throttledRecompute();
        }

        private void Recompute()
        {
            var newSubscriptions = new HashSet<INotifyPropertyChanged>();

            Computed.Listeners.Push(o => newSubscriptions.Add(o));

            T newVal;
            try
            {
                newVal = _compute();
            }
            finally
            {
                Computed.Listeners.Pop();
            }

            ValueImpl = newVal;
            newSubscriptions.Remove(this);

            foreach (var sub in _subscriptions.Where(s => !newSubscriptions.Contains(s)))
                sub.PropertyChanged -= RecomputeSoon;
        
            foreach (var sub in newSubscriptions.Where(s => !_subscriptions.Contains(s)))
                sub.PropertyChanged += RecomputeSoon;

            _subscriptions = newSubscriptions;
        }

        public T Value
        {
            get
            {
                if (_listenerCount != 0)
                {
                    return ValueImpl;
                }

                Computed.Listeners.Notify(this);
                return _compute();
            }
        } 

        private void Cleanup()
        {
            var subs = _subscriptions;
            _subscriptions = Computed.EmptySubscriptions;

            foreach (var sub in subs)
                sub.PropertyChanged -= RecomputeSoon;
        }

        public bool IsActive => _listenerCount != 0;

        private void ListenerCountChange(bool add)
        {
            if (add)
            {
                if (_listenerCount++ == 0)
                {
                    Recompute();
                }
            }
            else
            {
                if (--_listenerCount == 0)
                {
                    Cleanup();
                }
            }
        }
        
        public override event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                ListenerCountChange(true);
                base.PropertyChanged += value;                
            }

            remove
            {
                base.PropertyChanged -= value;
                ListenerCountChange(false);
            }
        }

        public void Dispose()
        {
            Cleanup();
        }
    }

    public static class Computed
    {
        internal static readonly ISet<INotifyPropertyChanged> EmptySubscriptions = new HashSet<INotifyPropertyChanged>();
        internal static readonly ListenerStack<INotifyPropertyChanged> Listeners = new ListenerStack<INotifyPropertyChanged>();

        public struct Void { }

        public static Computed<Void> Do(Action compute)
        {
            return new Computed<Void>(() =>
                {
                    compute();
                    return new Void();
                });
        }

        public static Computed<T> From<T>(Func<T> compute)
        {
            return new Computed<T>(compute);
        }

        public static SetableComputed<T> From<T>(Func<T> get, Action<T> set)
        {
            return new SetableComputed<T>(get, set);
        }

        public static T Throttle<T>(this ICanThrottle<T> computed, TimeSpan interval, bool waitForStable = true)
        {
            return computed.SetThrottler(a => Throttle(interval, waitForStable, a));
        }

        public static Action Throttle(TimeSpan interval, bool waitForStable, Action action)
        {
            DispatcherTimer timer = null;

            return () =>
            {
                if (timer != null)
                {
                    if (!waitForStable)
                        return;

                    timer.Stop();
                }

                timer = new DispatcherTimer { Interval = interval };
                timer.Tick += (sender, args) =>
                {
                    timer.Stop();
                    timer = null;
                    action();
                };
                timer?.Start();
            };
        }
    }
}