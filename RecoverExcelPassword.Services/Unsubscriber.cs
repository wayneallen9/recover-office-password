using System;
using System.Collections.Generic;

namespace RecoverExcelPassword.Services
{
    public class Unsubscriber<T> : IDisposable
    {
        #region variables
        private readonly IObserver<T> _observer;
        private readonly IList<IObserver<T>> _observers;
        private readonly System.IObserver<T> _systemObserver;
        private readonly IList<System.IObserver<T>> _systemObservers;
        #endregion

        public Unsubscriber(IList<IObserver<T>> observers, IObserver<T> observer)
        {
            _observer = observer;
            _observers = observers;
        }

        public Unsubscriber(IList<System.IObserver<T>> observers, System.IObserver<T> observer)
        {
            _systemObservers = observers;
            _systemObserver = observer;
        }

        public void Dispose()
        {
            if (_observers?.Contains(_observer) ?? false) _observers.Remove(_observer);
            if (_systemObservers?.Contains(_systemObserver) ?? false) _systemObservers.Remove(_systemObserver);
        }
    }
}