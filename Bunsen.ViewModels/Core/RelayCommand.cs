using Bunsen.API;
using System;

namespace Bunsen.ViewModels.Core
{
    /// <summary>
    /// Inspired from the MvvmLight library (lbugnion/MvvmLight)
    /// </summary>
    public sealed class RelayCommand : IRelayCommand
    {
        private readonly Action _execute;

        private readonly Func<bool>? _canExecute;

        public event EventHandler CanExecuteChanged = delegate { };

        public RelayCommand(Action execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ??
                throw new ArgumentNullException(nameof(canExecute));
        }

        public void NotifyCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke() != false;
        }

        public void Execute(object parameter)
        {
            _execute();
        }

    }
}
