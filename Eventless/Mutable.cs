using System.ComponentModel;

namespace Eventless
{
    public class Mutable<T> : MutableImpl<T>, IMutable<T>
    {
        public Mutable(T value = default(T))
            : base(value) { }

        public T Value
        {
            get { return ValueImpl; }
            set { ValueImpl = value; }
        }
    }

    public static class Mutable
    {
        public static Mutable<T> From<T>(T initVal)
        {
            return new Mutable<T>(initVal);
        }

        public static PropertyChangedEventArgs EventArgs = new PropertyChangedEventArgs(nameof(IImmutable<int>.Value));
    }
}