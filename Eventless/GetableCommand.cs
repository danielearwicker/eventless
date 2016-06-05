using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Eventless
{
    public class GetableCommand : ICommand, IDisposable
    {
        private readonly Action _execute;
        private readonly IGetable<bool> _canExecute;

        public GetableCommand(Action execute, IGetable<bool> canExecute = null)
        {
            _execute = execute;
            if (canExecute != null)
            {
                _canExecute = canExecute;
                _canExecute.PropertyChanged += CanExecutePropertyChanged;
            }
        }

        public void Dispose()
        {
            if (_canExecute != null)
            {
                _canExecute.PropertyChanged -= CanExecutePropertyChanged;
            }
        }

        private void CanExecutePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, e);
        }        

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Value ?? true;
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public event EventHandler CanExecuteChanged;        
    }
}