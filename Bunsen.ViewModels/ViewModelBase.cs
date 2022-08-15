using Bunsen.API;
using Bunsen.Core;
using System;

namespace Bunsen.ViewModels
{
    public abstract class ViewModelBase : ObservableObject
    {
        public ViewModelBase(IDataService dataService)
        {
            DataService = dataService ??
                throw new ArgumentNullException(nameof(dataService));
        }

        public bool CanExecute = true; // TODO add busy state

        protected IDataService DataService { get; }

    }
}
