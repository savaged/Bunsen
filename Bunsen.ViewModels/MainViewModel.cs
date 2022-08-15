using Bunsen.API;
using Bunsen.Models;
using Bunsen.ViewModels.Core;
using System.Threading.Tasks;

namespace Bunsen.ViewModels
{
    public class MainViewModel : ViewModelBase, IAsyncLoading
    {
        public MainViewModel(IDataService dataService) : base(dataService)
        {
            ScenarioStepLogViewModel = new ScenarioStepLogViewModel(dataService);
            ScenarioStepLogsViewModel = new ScenarioStepLogsViewModel(dataService);

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
