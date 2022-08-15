using Bunsen.API;
using Bunsen.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bunsen.ViewModels.Core;
using System;

namespace Bunsen.ViewModels
{
    public class ScenarioStepLogsViewModel : ViewModelBase, IAsyncLoading
    {
        public ScenarioStepLogsViewModel(
            IBusyStateManager busyStateManager,
            IDataService dataService)
            : base(busyStateManager, dataService)
        {
            Index = new List<ScenarioStepLog>();
            AddCmd = new RelayCommand(OnAdd, () => CanExecuteAdd);
        }

        public async Task LoadAsync()
        {
            await LoadIndexAsync();
        }

        public event EventHandler<AddingModelEventArgs>? AddingModel;

        public IList<ScenarioStepLog> Index { get; }

        public IRelayCommand AddCmd { get; }

        public bool CanExecuteAdd => CanExecute;

        private async Task LoadIndexAsync()
        {
            Index.Clear();
            BusyStateManager.RegisterBusiness();
            try
            {
                var data = await DataService.IndexAsync<ScenarioStepLog>();
                foreach (var model in data)
                {
                    Index.Add(model);
                }
                NotifyPropertyChanged(nameof(Index));
            }
            finally
            {
                BusyStateManager.UnregisterBusiness();
            }
        }

        private void OnAdd()
        {
            var model = new ScenarioStepLog();
            Index.Add(model);
            NotifyPropertyChanged(nameof(Index));
            AddingModel?.Invoke(this, new AddingModelEventArgs(model));
        }

    }
}
