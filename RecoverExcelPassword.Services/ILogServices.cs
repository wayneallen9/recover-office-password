using System;

namespace RecoverExcelPassword.Services
{
    public interface ILogServices
    {
        void Error(Exception ex);
        void Trace(string message);
        void TraceEnter();
        void TraceExit();
        void Warn(string message);
    }
}