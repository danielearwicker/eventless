using System;

namespace Eventless
{
    public interface IGetable<out T> : IGetable
    {
        T Value { get; }
    }

    public interface IGetable
    {
        event Action Changed;
    }
}