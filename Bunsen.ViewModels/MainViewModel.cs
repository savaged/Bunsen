using Bunsen.API;
using Bunsen.Models;
using Bunsen.ViewModels.Core;
using System.Threading.Tasks;

namespace Bunsen.ViewModels
{
    public class MainViewModel : ViewModelBase, IAsyncLoading
    {
        public MainViewModel(
            IBusyStateManager busyStateManager,
            IDataService dataService)
            : base(busyStateManager, dataService)
        {
            ScenarioStepLogViewModel = new ScenarioStepLogViewModel(busyStateManager, dataService);
            ScenarioStepLogsViewModel = new ScenarioStepLogsViewModel(busyStateManager, dataService);

            ScenarioStepLogsViewModel.AddingModel += OnAddingModel;
        }

        public ScenarioStepLogViewModel ScenarioStepLogViewModel { get; }
        public ScenarioStepLogsViewModel ScenarioStepLogsViewModel { get; }

        public async Task LoadAsync()
        {
            await ScenarioStepLogsViewModel.LoadAsync();
        }

        private void OnAddingModel(object sender, AddingModelEventArgs e)
        {
            ScenarioStepLogViewModel.SelectedItem = e.Model as ScenarioStepLog;
        }

    }
}
