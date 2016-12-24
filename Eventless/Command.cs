using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Eventless
{
    /// <summary>
    /// An implementation of <see cref="ICommand"/> that determines whether it
    /// can execute by referring to an <see cref="IImmutable{T}"/>, which could
    /// be a <see cref="Computed{T}"/> for example.
    /// </summary>
    public class Command : ICommand, IDisposable
    {
        private readonly Action _execute;
        private readonly IImmutable<bool> _canExecute;

        /// <summary>
        /// Constructs a <see cref="Computed{T}"/>
        /// </summary>
        /// <param name="execute">The action to execute</param>
        /// <param name="canExecute">The observable that indicates if the action is currently allowed</param>
        public Command(Action execute, IImmutable<bool> canExecute = null)
        {
            _execute = execute;
            if (canExecute != null)
            {
                _canExecute = canExecute;
                _canExecute.PropertyChanged += CanExecutePropertyChanged;
            }
        }

        /// <summary>
        /// Cleans up the command so it no longer listens for changes.
        /// </summary>
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

        /// <summary>
        /// See <see cref="ICommand.CanExecute"/>. The <c>parameter</c> is ignored; instead,
        /// declare your commands so that they get the information they required from
        /// their environment.
        /// </summary>
        /// <param name="parameter">Ignored</param>
        /// <returns><c>true</c> if the command can execute</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute?.Value ?? true;
        }

        /// <summary>
        /// See <see cref="ICommand.Execute"/>. The <c>parameter</c> is ignored; instead,
        /// declare your commands so that they get the information they required from
        /// their environment.
        /// </summary>
        /// <param name="parameter">Ignored</param>
        public void Execute(object parameter)
        {
            _execute();
        }

        /// <summary>
        /// See <see cref="ICommand.CanExecuteChanged"/>.
        /// </summary>
        public event EventHandler CanExecuteChanged;
    }
}