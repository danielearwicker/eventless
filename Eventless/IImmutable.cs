using System.ComponentModel;

namespace Eventless
{
    public interface IImmutable<out T> : INotifyPropertyChanged
    {
        T Value { get; }
    }    
}