using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Threading;

namespace Eventless
{
    /// <summary>
    /// A derived value, defined by an expression that can consume values from other observables,
    /// including <see cref="Mutable{T}"/> values and other <see cref="Computed{T}"/> values.
    /// When the other values change, the <c>compute</c> expression is re-evaluated to
    /// update the value automatically. This in turn will notify other listeners.
    /// </summary>
    /// <typeparam name="T">The value type</typeparam>
    public class Computed<T> : MutableImpl<T>, IImmutable<T>, ICanThrottle<Computed<T>>, IDisposable
    {
        private readonly Func<T> _compute;

        private ISet<INotifyPropertyChanged> _subscriptions = Computed.EmptySubscriptions;

        private Action _throttledRecompute;

        private int _listenerCount;

        /// <summary>
        /// Constructs a <see cref="Computed{T}"/> from the specified <see cref="Func{TResult}"/>.
        /// </summary>
        /// <param name="compute">Computes the current value, typically from other observables</param>
        public Computed(Func<T> compute)
            : this(compute, 0) { }

        internal Computed(Func<T> compute, int listenerCount)
            : base(new Mutable<T>())
        {
            _listenerCount = listenerCount;
            _compute = compute;
            _throttledRecompute = Recompute;

            if (_listenerCount != 0)
            {
                Recompute();
            }
        }

        /// <summary>
        /// See <see cref="ICanThrottle{T}.SetThrottler"/>
        /// </summary>
        /// <param name="throttler">The throttling behaviour</param>
        /// <returns>This <see cref="Computed{T}"/> object</returns>
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

        /// <summary>
        /// The current value of the <c>compute</c> <see cref="Func{TResult}"/>.
        /// </summary>
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

        /// <summary>
        /// Has the value <c>true</c> if there are currently any listeners to
        /// this object's <see cref="INotifyPropertyChanged.PropertyChanged"/> event.        
        /// </summary>
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
        
        /// <summary>
        /// Raised whenever the value changes. If there are no listeners subscribed
        /// to this event, this <see cref="Computed{T}"/> will unsubscribe from all
        /// the observables it depends on.
        /// </summary>
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

        /// <summary>
        /// Forces this object to unsubscribe from all the observables it depends on.
        /// </summary>
        public void Dispose()
        {
            Cleanup();
        }
    }

    /// <summary>
    /// Static utilities relating to <see cref="Computed{T}"/> observables.
    /// </summary>
    public static class Computed
    {
        internal static readonly ISet<INotifyPropertyChanged> EmptySubscriptions = new HashSet<INotifyPropertyChanged>();
        internal static readonly ListenerStack<INotifyPropertyChanged> Listeners = new ListenerStack<INotifyPropertyChanged>();
        private struct Void { }

        /// <summary>
        /// Registers an action to re-execute whenever there is a change in the value of
        /// any observable that it depends on.
        /// 
        /// It's important to dispose of it when you want it to stop executing, as its
        /// lifetime will be extended by the lifetimes of the observables it depends on.
        /// </summary>
        /// <param name="action">The action to execute</param>
        /// <returns>An object that can be disposed to stop further execution of the action</returns>
        public static IDisposable Do(Action action)
        {
            return new Computed<Void>(() =>
                {
                    action();
                    return new Void();
                }, 1);
        }

        /// <summary>
        /// Returns a <see cref="Computed{T}"/>. The advantage of this
        /// over calling the constructor directly is that it can infer the value
        /// type from the return type of <c>compute</c>, so you can say:
        /// <code>Computed.From(() => -Counter.Value)</code>
        /// instead of:
        /// <code>new Computed&lt;int&gt;(() => -Counter.Value)</code>
        /// </summary>
        /// <param name="compute">The <see cref="Func{TResult}"/> that computes the current value</param>
        /// <typeparam name="T">The value type</typeparam>
        /// <returns>A <see cref="Computed{T}"/></returns>
        public static Computed<T> From<T>(Func<T> compute)
        {
            return new Computed<T>(compute);
        }

        /// <summary>
        /// Returns a <see cref="MutableComputed{T}"/>
        /// </summary>
        /// <param name="get">The <see cref="Func{TResult}"/> that computes the current value</param>
        /// <param name="set">The <see cref="Action{T}"/> that stores a new value</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>A <see cref="MutableComputed{T}"/></returns>
        public static MutableComputed<T> From<T>(Func<T> get, Action<T> set)
        {
            return new MutableComputed<T>(get, set);
        }

        /// <summary>
        /// Sets throttling behaviour on an object that supports it (such as <see cref="Computed{T}"/>).
        /// 
        /// The optional <c>waitForStable</c> pararameter controls the behaviour. If <c>true</c>
        /// then the value must remain constant for the specified time <c>interval</c> before
        /// a re-evaluation can occur. Otherwise, re-evaluations are allowed to occur repeatedly at the
        /// specified time <c>interval</c>.
        /// </summary>
        /// <param name="computed">The object to throttle</param>
        /// <param name="interval">The minimum time to wait before re-evaluating</param>
        /// <param name="waitForStable">Whether to wait for a stable value</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>The object being throttled, allowing chaining</returns>
        public static T Throttle<T>(this ICanThrottle<T> computed, TimeSpan interval, bool waitForStable = true)
        {
            return computed.SetThrottler(a => Throttle(interval, waitForStable, a));
        }

        /// <summary>
        /// The implementation used by the <see cref="Throttle{T}"/> extension method.
        /// </summary>
        /// <param name="interval">The minimum time to wait before re-evaluating</param>
        /// <param name="waitForStable">Whether to wait for a stable value</param>
        /// <param name="action">The action to be throttled</param>
        /// <returns>A throttled version of the specified action</returns>
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