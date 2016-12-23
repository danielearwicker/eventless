using System;

namespace Eventless
{
    public sealed class MutableComputed<T> : Computed<T>, IMutable<T>
    {
        private readonly Action<T> _set;

        public MutableComputed(Func<T> get, Action<T> set)
            : base(get)
        {
            _set = set;
        }

        public new T Value
        {
            get { return base.Value; }
            set { _set(value); }
        }
    }
}
