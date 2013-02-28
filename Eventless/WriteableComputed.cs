using System;

namespace Eventless
{
    public sealed class WriteableComputed<T> : Forwarder<Computed<T>, T>, IWriteable<T>
    {
        private readonly Action<T> _set;

        public WriteableComputed(Func<T> get, Action<T> set)
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
