using System.ComponentModel;

namespace Eventless
{
    /// <summary>
    /// A single value that can be read but not written, via the <see cref="Value"/> property.
    /// It inherits <see cref="INotifyPropertyChanged"/> so it can raise an event whenever the
    /// value changes. This means that XAML binding expressions can target <see cref="Value"/>.
    /// </summary>
    /// <typeparam name="T">The value type</typeparam>
    public interface IImmutable<out T> : INotifyPropertyChanged
    {
        /// <summary>
        /// The current value of the immutable object.
        /// </summary>
        T Value { get; }
    }    
}