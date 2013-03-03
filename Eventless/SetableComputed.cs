using System;

namespace Eventless
{
    public sealed class SetableComputed<T> : Forwarder<Computed<T>, T>, ISetable<T>, ICanThrottle<SetableComputed<T>>
    {
        private readonly Action<T> _set;

        public SetableComputed(Func<T> get, Action<T> set)
            : base(new Computed<T>(get))
        {
            _set = set;
        }

        public T Value
        {
            get { return Impl.Value; }
            set { _set(value); }
        }

        public SetableComputed<T> SetThrottler(Func<Action, Action> throttler)
        {
            Impl.SetThrottler(throttler);
            return this;
        }
    }
}
