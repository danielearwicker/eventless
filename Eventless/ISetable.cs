namespace Eventless
{
    public interface ISetable<T> : IGetable<T>
    {
        new T Value { get; set; }
    }
}