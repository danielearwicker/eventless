using System;
using System.ComponentModel;

namespace Eventless
{
    /// <summary>
    /// A variant of <see cref="Computed{T}"/> that allows assignment to its
    /// <see cref="MutableComputed{T}.Value"/> property. The meaning has to
    /// be specified by a custom <c>set</c> <see cref="Action"/> that 
    /// accepts the value and somehow stores it, ideally by doing the inverse 
    /// of the <c>get</c> action.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class MutableComputed<T> : Computed<T>, IMutable<T>
    {
        private readonly Action<T> _set;

        /// <summary>
        /// Constructs a <see cref="MutableComputed{T}"/>.
        /// </summary>
        /// <param name="get">The function to evaluate to compute the current value</param>
        /// <param name="set">The action to execute to store a new value</param>
        public MutableComputed(Func<T> get, Action<T> set)
            : base(get)
        {
            _set = set;
        }

        /// <summary>
        /// Allows the current value to be read or updated.
        /// </summary>
        public new T Value
        {
            get { return base.Value; }
            set { _set(value); }
        }
    }
}
