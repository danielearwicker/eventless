using System;

namespace Eventless
{
    public interface IEquate<T>
    {
        Func<T, T, bool> EqualityComparer { get; set; }
    }
}