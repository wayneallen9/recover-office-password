using System;

namespace RecoverExcelPassword.Services
{
    public interface IObservable<T>
    {
        IDisposable Subscribe(IObserver<T> observer);
    }
}