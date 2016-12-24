using System.ComponentModel;

namespace Eventless
{
    /// <summary>
    /// Represents a single value that can be updated as well as read.
    /// </summary>
    /// <typeparam name="T">The value type</typeparam>
    public class Mutable<T> : MutableImpl<T>, IMutable<T>
    {
        /// <summary>
        /// Constructs a <see cref="Mutable{T}"/>, initializing its value.
        /// </summary>
        /// <param name="value">The optional value to initialize to</param>
        public Mutable(T value = default(T))
            : base(value) { }

        /// <summary>
        /// See <see cref="IMutable{T}.Value"/>.
        /// </summary>
        public T Value
        {
            get { return ValueImpl; }
            set { ValueImpl = value; }
        }
    }

    /// <summary>
    /// Static helpers for working with <see cref="Mutable{T}"/> objects.
    /// </summary>
    public static class Mutable
    {
        /// <summary>
        /// Creates an instance of <see cref="Mutable{T}"/>. The advantage of this
        /// over calling the constructor directly is that it can infer the value
        /// type from the initializer value, so you can say:
        /// <code>Mutable.From(5)</code>
        /// instead of:
        /// <code>new Mutable&lt;int&gt;(5)</code>
        /// </summary>
        /// <param name="initVal">The initial value</param>
        /// <typeparam name="T">The value type</typeparam>
        /// <returns>A <see cref="Mutable{T}"/> storing the value</returns>
        public static Mutable<T> From<T>(T initVal)
        {
            return new Mutable<T>(initVal);
        }

        /// <summary>
        /// When <see cref="Mutable{T}"/> raises the <see cref="INotifyPropertyChanged.PropertyChanged"/>
        /// event, it always specifies the same name, <c>"Value"</c>. To avoid allocating a new
        /// object every time, it simply raises this one.
        /// </summary>
        public static readonly PropertyChangedEventArgs EventArgs = new PropertyChangedEventArgs(nameof(IImmutable<int>.Value));
    }
}
