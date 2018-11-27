namespace RecoverExcelPassword.Services
{
    public interface IDesktopActions
    {
        void StartWatching();
        void StopWatching();
        void Subscribe(IDesktopActionsObserver observer);
    }
}