using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace Eventless
{
    /// <summary>
    /// Defines logic for maintaining a mutable value, common to <see cref="Mutable{T}"/>
    /// and <see cref="Computed{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MutableImpl<T> : IEquate<T>, INotifyPropertyChanged 
    {
        private T _value;
        private bool _changing;

        /// <summary>
        /// Constructs an initialized <see cref="Mutable{T}"/>.
        /// </summary>
        /// <param name="value">The initial value</param>
        protected MutableImpl(T value = default(T))
        {
            _value = value;
        }

        /// <summary>
        /// Gets or sets the current stored value.
        /// </summary>
        /// <exception cref="RecursiveModificationException"></exception>
        protected T ValueImpl
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
                _changing = true;

                try
                {
                    PropertyChanged?.Invoke(this, Mutable.EventArgs);
                }
                finally
                {
                    _changing = false;
                }
            }
        }
        
        /// <summary>
        /// Implements <see cref="INotifyPropertyChanged"/>, providing a way to notify
        /// any dependents that the value has changed.
        /// </summary>
        public virtual event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// See <see cref="IEquate{T}.EqualityComparer"/>.
        /// </summary>
        public Func<T, T, bool> EqualityComparer { get; set; } = DefaultEqualityComparer;

        /// <summary>
        /// The default setting for <see cref="EqualityComparer"/>, suitable for most purposes.
        /// </summary>
        /// <param name="a">The first of the values to compare</param>
        /// <param name="b">The second of the value to compare</param>
        /// <returns>Indicates whether the values were equal</returns>
        public static bool DefaultEqualityComparer(T a, T b)
        {
            return EqualityComparer<T>.Default.Equals(a, b);
        }

        /// <summary>
        /// Conversion to the value type. This avoids the need to use the <c>.Value</c>
        /// suffix in some situations (though sadly not in XAML binding expressions).
        /// </summary>
        /// <param name="from">The object to convert from</param>
        /// <returns>The value obtained from the object</returns>
        public static implicit operator T(MutableImpl<T> from)
        {
            return from.ValueImpl;
        }
    }
}