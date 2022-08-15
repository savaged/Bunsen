using Bunsen.API;
using Bunsen.Models;
using Bunsen.ViewModels.Core;

namespace Bunsen.ViewModels
{
    public class ScenarioStepLogViewModel : ViewModelBase
    {
        private ScenarioStepLog? _selectedItem;

        public ScenarioStepLogViewModel(
            IBusyStateManager busyStateManager,
            IDataService dataService)
            : base(busyStateManager, dataService)
        {
            SaveCmd = new RelayCommand(OnSave, () => CanExecuteSave);
            DeleteCmd = new RelayCommand(OnDelete, () => CanExecuteDelete);
        }

        public ScenarioStepLog? SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged(nameof(SelectedItem));
            }
        }

        public IRelayCommand SaveCmd { get; }
        public IRelayCommand DeleteCmd { get; }

        public bool CanExecuteSave => CanExecute && IsItemSelected;
        public bool CanExecuteDelete => CanExecute && IsItemSelected;

        public bool IsItemSelected => SelectedItem != null;

        private async void OnSave()
        {
            if (SelectedItem == null) return;

            BusyStateManager.RegisterBusiness();
            try
            {
                if (SelectedItem?.Id == 0)
                {
                    SelectedItem = await DataService!.StoreAsync(SelectedItem!);
                }
                else
                {
                    await DataService!.UpdateAsync(SelectedItem!);
                }
            }
            finally
            {
                BusyStateManager.UnregisterBusiness();
            }
        }

        private async void OnDelete()
        {
            if (SelectedItem == null) return;

            BusyStateManager.RegisterBusiness();
            try
            {
                await DataService!.DeleteAsync<ScenarioStepLog>(SelectedItem!.Id);
                SelectedItem = null;
            }
            finally
            {
                BusyStateManager.UnregisterBusiness();
            }
        }

    }
}
