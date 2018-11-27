using System;

namespace RecoverExcelPassword.Services
{
    public interface IObserver<T>
    {
        void Error(Exception ex);
        void Update(T observable);
    }
}