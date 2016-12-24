using System.ComponentModel;

namespace Eventless
{
    /// <summary>
    /// A single value that can written to as well as read. It inherits <see cref="INotifyPropertyChanged"/>
    /// via <see cref="IImmutable{T}"/>, so binding expressions can target the <see cref="Value"/> property
    /// for two-way binding.
    /// </summary>
    /// <typeparam name="T">The value type</typeparam>
    public interface IMutable<T> : IImmutable<T>
    {
        /// <summary>
        /// Allows the current value to be read or updated. Changing the value causes
        /// the <see cref="INotifyPropertyChanged.PropertyChanged"/> event to be raised.
        /// </summary>
        new T Value { get; set; }
    }
}