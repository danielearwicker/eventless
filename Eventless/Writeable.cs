using System;

namespace Eventless
{
    public sealed class Writeable<T> : IWriteable<T>, IEquate<T>
    {
        private T _value;
        private bool _changing;

        public Writeable(T value = default(T))
        {
            _value = value;
            EqualityComparer = DefaultEqualityComparer;
        }

        public T Value
        {
            get
            {
                Computed.Listeners.Notify(this);
                return _value;
            }

            set
            {
                if (EqualityComparer(_value, value))
                    return;

                if (_changing)
                    throw new RecursiveModificationException();

                _value = value;

                var evt = Changed;
                if (evt == null) 
                    return;

                _changing = true;
                try { evt(); } 
                finally { _changing = false; }
            }
        }

        public event Action Changed;

        public Func<T, T, bool> EqualityComparer { get; set; }

        public static bool DefaultEqualityComparer(T a, T b)
        {
            return ReferenceEquals(a, b) || (!ReferenceEquals(a, null) && a.Equals(b));
        }

        public static implicit operator T(Writeable<T> from)
        {
            return from.Value;
        }
    }

    public static class Writeable
    {
        public static Writeable<T> From<T>(T initVal)
        {
            return new Writeable<T>(initVal);
        }
    }
}