namespace RecoverExcelPassword.Services
{
    public interface IWindowFinder : System.IObservable<Models.Window>
    {
        void Find();
        void Find(int processId);
    }
}