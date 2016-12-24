using System;

namespace Eventless
{
    /// <summary>
    /// Exception thrown when the modification of a <see cref="Mutable{T}"/>'s <c>Value</c>
    /// property cause a change reaction of updates that results in the same <see cref="Mutable{T}"/>
    /// being modified again. This indicates a bug in the application logic.
    /// </summary>
    public sealed class RecursiveModificationException : Exception
    {
        /// <summary>
        /// Constructs the exception.
        /// </summary>
        public RecursiveModificationException()
            : base("Recursive modification of Mutable")
        {
        }
    }
}