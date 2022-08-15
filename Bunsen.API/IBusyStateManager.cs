namespace Bunsen.API
{
    public interface IBusyStateManager
    {
        bool IsBusy { get; }

        void RegisterBusiness();
        void UnregisterBusiness();
    }

}
