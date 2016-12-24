using System;

namespace Eventless
{
    /// <summary>
    /// Implemented by objects that can be "throttled", which means to limit the
    /// rate at which operations may be repeated. It's usually neater to use the
    /// <see cref="Computed.Throttle{T}"/> extension method instead of calling
    /// <see cref="SetThrottler"/> directly.
    /// </summary>
    /// <typeparam name="T">The concrete type that implements this interface</typeparam>
    public interface ICanThrottle<out T>
    {
        /// <summary>
        /// Assigns a new throttling wrapper. The throttler is a function that
        /// accepts a plain <see cref="Action"/> and returns another action that
        /// has been appropriately throttled, i.e. made to batch up executions
        /// so they do not occur too rapidly. 
        /// </summary>
        /// <param name="throttler">The desired throttling behaviour</param>
        /// <returns>The object on which this assignment was made</returns>
        T SetThrottler(Func<Action, Action> throttler);
    }
}