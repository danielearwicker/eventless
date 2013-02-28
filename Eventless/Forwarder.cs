using System;

namespace Eventless
{
    public class Forwarder<TImpl, TValue> : IReadable, IEquate<TValue> where TImpl : class, IEquate<TValue>, IReadable
    {
        private readonly TImpl _impl;

        public Forwarder(TImpl impl)
        {
            _impl = impl;
        }

        protected TImpl Impl
        {
            get { return _impl; }
        }

        public event Action Changed
        {
            add { _impl.Changed += value; }
            remove { _impl.Changed -= value; }
        }

        public Func<TValue, TValue, bool> EqualityComparer
        {
            get { return _impl.EqualityComparer; }
            set { _impl.EqualityComparer = value; }
        }
    }
}