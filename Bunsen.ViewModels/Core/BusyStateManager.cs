using Bunsen.API;
using Bunsen.Core;

namespace Bunsen.ViewModels.Core
{
    public sealed class BusyStateManager : ObservableObject, IBusyStateManager
    {
        private static volatile BusyStateManager? _instance;
        private static readonly object _lock = new object();
        private int _busyCount = 0;

        private BusyStateManager() { }

        public static BusyStateManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new BusyStateManager();
                    }
                }
                return _instance;
            }
        }

        public void RegisterBusiness()
        {
            _busyCount++;
            NotifyPropertyChanged(nameof(IsBusy));
        }

        public void UnregisterBusiness()
        {
            if (_busyCount > 0) _busyCount--;
            NotifyPropertyChanged(nameof(IsBusy));
        }

        public bool IsBusy => _busyCount == 0;

    }
}
