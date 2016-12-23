namespace Eventless
{
    public interface IMutable<T> : IImmutable<T>
    {
        new T Value { get; set; }
    }
}