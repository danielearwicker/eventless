using System;
using System.Collections.Generic;
using System.Linq;

namespace Eventless
{
    public sealed class Computed<T> : Forwarder<Writeable<T>, T>, IReadable<T>, IDisposable
    {
        private readonly Func<T> _compute;

        private ISet<IReadable> _subscriptions = Computed.EmptySubscriptions;

        public Computed(Func<T> compute)
            : base(new Writeable<T>())
        {
            _compute = compute;
            Recompute();
        }

        private void Recompute()
        {
            var newSubscriptions = new HashSet<IReadable>();

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
        internal static readonly ISet<IReadable> EmptySubscriptions = new HashSet<IReadable>();
        internal static readonly ListenerStack<IReadable> Listeners = new ListenerStack<IReadable>();
        
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

        public static WriteableComputed<T> From<T>(Func<T> get, Action<T> set)
        {
            return new WriteableComputed<T>(get, set);
        }
    }
}