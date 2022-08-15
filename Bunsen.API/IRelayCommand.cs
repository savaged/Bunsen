using System.Windows.Input;

namespace Bunsen.API
{
    public interface IRelayCommand : ICommand
    {
        void NotifyCanExecuteChanged();
    }
}
