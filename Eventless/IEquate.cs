using System;
using System.Collections.Generic;

namespace Eventless
{
    /// <summary>
    /// Implemented by an object that requires a way to compare two values. The
    /// method of comparison can be any function that takes two values of the
    /// correct type and returns a <see cref="bool"/>.
    /// 
    /// Usually <see cref="EqualityComparer{T}.Default"/> is suitable for most
    /// purposes, and this is the default uses in Eventless, but sometimes it
    /// might be useful to override it.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEquate<T>
    {
        /// <summary>
        /// The function this object will use to compare values for equality.
        /// </summary>
        Func<T, T, bool> EqualityComparer { get; set; }
    }
}