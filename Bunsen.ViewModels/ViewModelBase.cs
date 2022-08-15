using Bunsen.API;
using Bunsen.Core;
using System;

namespace Bunsen.ViewModels
{
    public abstract class ViewModelBase : ObservableObject
    {
        public ViewModelBase(
            IBusyStateManager busyStateManager,
            IDataService dataService)
        {
            BusyStateManager = busyStateManager ??
                throw new ArgumentNullException(nameof(busyStateManager));
            DataService = dataService ??
                throw new ArgumentNullException(nameof(dataService));
        }

        protected IBusyStateManager BusyStateManager { get; }

        public bool CanExecute => BusyStateManager.IsBusy;

        protected IDataService DataService { get; }

    }
}
