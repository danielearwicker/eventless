namespace Eventless
{
    public interface IBindsTo<T>
    {
        void Bind(T context);
    }
}