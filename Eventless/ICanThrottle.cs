using System;

namespace Eventless
{
    public interface ICanThrottle<out T>
    {
        T SetThrottler(Func<Action, Action> throttler);
    }
}