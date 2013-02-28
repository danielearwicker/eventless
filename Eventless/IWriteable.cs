namespace Eventless
{
    public interface IWriteable<T> : IReadable<T>
    {
        new T Value { get; set; }
    }
}