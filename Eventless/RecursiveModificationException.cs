using System;

namespace Eventless
{
    public sealed class RecursiveModificationException : Exception
    {
        public RecursiveModificationException()
            : base("Recursive modification of Writeable")
        {
        }
    }
}