using System;

namespace RecoverExcelPassword
{
    public interface IInvoker
    {
        bool InvokeRequired {get;}
        object Invoke(Delegate delegateMethod, params object[] args);
    }
}