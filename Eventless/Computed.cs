using System;
using System.Collections.Generic;
using System.Linq;

namespace Eventless
{
    public sealed class Computed<T> : Forwarder<Setable<T>, T>, IGetable<T>, IDisposable
    {
        private readonly Func<T> _compute;

        private ISet<IGetable> _subscriptions = Computed.EmptySubscriptions;

        public Computed(Func<T> compute)
            : base(new Setable<T>())
        {
            _compute = compute;
            Recompute();
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
                sub.Changed -= Recompute;
        
            foreach (var sub in newSubscriptions.Where(s => !_subscriptions.Contains(s)))
                sub.Changed += Recompute;
        
            _subscriptions = newSubscriptions;
        }

        public T Value
        {
            get { return Impl.Value; }
        }

        public void Dispose()
        {
            foreach (var sub in _subscriptions)
                sub.Changed -= Recompute;
        }
    }

    public static class Computed
    {
        internal static readonly ISet<IGetable> EmptySubscriptions = new HashSet<IGetable>();
        internal static readonly ListenerStack<IGetable> Listeners = new ListenerStack<IGetable>();
        
        public static void Do(Action compute)
        {
            new Computed<int>(() =>
                {
                    compute();
                    return 0;
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
    }
}