using System;

namespace Eventless
{
    public sealed class SetableComputed<T> : Forwarder<Computed<T>, T>, ISetable<T>
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
    }
}
