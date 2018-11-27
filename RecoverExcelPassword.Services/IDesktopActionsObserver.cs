using System;

namespace RecoverExcelPassword.Services
{
    public interface IDesktopActionsObserver
    {
        void OnClick(IntPtr hWnd);
    }
}