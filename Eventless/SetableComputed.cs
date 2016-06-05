using System;

namespace Eventless
{
    public sealed class SetableComputed<T> : Computed<T>, ISetable<T>
    {
        private readonly Action<T> _set;

        public SetableComputed(Func<T> get, Action<T> set)
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
