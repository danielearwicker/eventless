using System;
using System.ComponentModel;

namespace Eventless
{
    public interface IGetable<out T> : INotifyPropertyChanged
    {
        T Value { get; }
    }    
}