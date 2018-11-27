using System;

namespace RecoverExcelPassword.Services
{
    public interface IWindowServices
    {
        bool TryPassword(IntPtr windowHandle, string password);
    }
}