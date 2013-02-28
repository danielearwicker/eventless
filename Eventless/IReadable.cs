using System;

namespace Eventless
{
    public interface IReadable<out T> : IReadable
    {
        T Value { get; }
    }

    public interface IReadable
    {
        event Action Changed;
    }
}