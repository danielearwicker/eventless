using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventless
{
    public sealed class Computed<T> : Forwarder<Setable<T>, T>, IGetable<T>, IDisposable, ICanThrottle<Computed<T>>
    {
        private readonly Func<T> _compute;

        private ISet<IGetable> _subscriptions = Computed.EmptySubscriptions;

        private Action _throttledRecompute;

        public Computed(Func<T> compute)
            : base(new Setable<T>())
        {
            _compute = compute;
            _throttledRecompute = Recompute;
            Recompute();
        }

        public Computed<T> SetThrottler(Func<Action, Action> throttler)
        {
            _throttledRecompute = throttler(Recompute);
            return this;
        }

        private void RecomputeSoon()
        {
            _throttledRecompute();
        }

        private void Recompute()
        {
            var newSubscriptions = new HashSet<IGetable>();

            Computed.Listeners.Push(o => newSubscriptions.Add(o));
            var newVal = _compute();
            Computed.Listeners.Pop();
            Impl.Value = newVal;
            newSubscriptions.Remove(Impl);

            foreach (var sub in _subscriptions.Where(s => !newSubscriptions.Contains(s)))
                sub.Changed -= RecomputeSoon;
        
            foreach (var sub in newSubscriptions.Where(s => !_subscriptions.Contains(s)))
                sub.Changed += RecomputeSoon;
        
            _subscriptions = newSubscriptions;
        }

        public T Value
        {
            get { return Impl.Value; }
        }

        public void Dispose()
        {
            foreach (var sub in _subscriptions)
                sub.Changed -= RecomputeSoon;
        }
    }

    public static class Computed
    {
        internal static readonly ISet<IGetable> EmptySubscriptions = new HashSet<IGetable>();
        internal static readonly ListenerStack<IGetable> Listeners = new ListenerStack<IGetable>();

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

        public static AsyncComputed<T> From<T>(Func<Task<T>> asyncObs)
        {
            return new AsyncComputed<T>(asyncObs);
        }
    }
}