using System.ComponentModel;

namespace Eventless
{
    public class Setable<T> : SetableImpl<T>, ISetable<T>
    {
        public Setable(T value = default(T))
            : base(value) { }

        public T Value
        {
            get { return ValueImpl; }
            set { ValueImpl = value; }
        }
    }

    public static class Setable
    {
        public static Setable<T> From<T>(T initVal)
        {
            return new Setable<T>(initVal);
        }

        public static PropertyChangedEventArgs EventArgs = new PropertyChangedEventArgs(nameof(IGetable<int>.Value));
    }
}