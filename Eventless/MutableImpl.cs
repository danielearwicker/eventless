using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace Eventless
{
    public class MutableImpl<T> : IEquate<T>, INotifyPropertyChanged 
    {
        private T _value;
        private bool _changing;

        protected MutableImpl(T value = default(T))
        {
            _value = value;
        }

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
        
        public virtual event PropertyChangedEventHandler PropertyChanged;

        public Func<T, T, bool> EqualityComparer { get; set; } = DefaultEqualityComparer;

        public static bool DefaultEqualityComparer(T a, T b)
        {
            return EqualityComparer<T>.Default.Equals(a, b);
        }

        public static implicit operator T(MutableImpl<T> from)
        {
            return from.ValueImpl;
        }
    }
}