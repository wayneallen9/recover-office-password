using AutoMapper;
using System;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace RecoverExcelPassword
{
    public class MainFormViewModel : INotifyPropertyChanged, System.IObserver<Services.Models.Window>
    {
        #region enumerations
        #endregion

        #region delegates
        private delegate void OnErrorDelegate(Exception ex);
        private delegate void OnNextDelegate(Services.Models.Window window);
        private delegate void OnPropertyChangedDelegate(string propertyName);
        #endregion

        #region events
        public event ErrorEventHandler Error;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region variables
        private bool _canRefresh;
        private bool _canStart;
        private readonly IInvoker _invoker;
        private readonly Services.ILogServices _logServices;
        private Models.Window _selectedWindow;
        private readonly Services.IWindowFinder _windowFinder;
        #endregion

        public MainFormViewModel(
            IInvoker invoker,
            Services.ILogServices logServices,
            Services.IWindowFinder windowFinder)
        {
            // save dependency injections
            _invoker = invoker;
            _logServices = logServices;
            _windowFinder = windowFinder;

            // initialise variables
            _canRefresh = false;
            _canStart = false;

            // watch for qualifying windows
            _windowFinder.Subscribe(this);
            Task.Factory.StartNew(() => FindWindowsThread())
                .ContinueWith(OnError, null, TaskContinuationOptions.OnlyOnFaulted);
        }

        public bool CanRefresh {
            get => _canRefresh;
            private set {
                _logServices.TraceEnter();
                try
                {
                    // only process changes
                    if (_canRefresh == value) return;

                    _logServices.Trace($"Setting value of {nameof(CanRefresh)}...");
                    _canRefresh = value;

                    OnPropertyChanged();
                }
                finally
                {
                    _logServices.TraceExit();
                }
            }
        }

        public bool CanStart
        {
            get => _canStart;
            private set
            {
                _logServices.TraceEnter();
                try
                {
                    // only process changes
                    if (_canStart == value) return;

                    _logServices.Trace($"Setting value of {nameof(CanStart)}...");
                    _canStart = value;

                    OnPropertyChanged();
                }
                finally
                {
                    _logServices.TraceExit();
                }
            }
        }

        private void FindWindowsThread()
        {
            _logServices.TraceEnter();
            try
            {
                _logServices.Trace($"Setting status flags...");
                CanRefresh = false;
                CanStart = false;

                _logServices.Trace("Searching for matching windows...");
                _windowFinder.Find();
            }
            finally
            {
                _logServices.TraceExit();
            }
        }

        public void OnError(Exception ex)
        {
            _logServices.TraceEnter();
            try
            {
                _logServices.Trace("Checking if running on UI thread...");
                if (_invoker.InvokeRequired)
                {
                    _logServices.Trace("Not running on UI thread.  Delegating to UI thread...");
                    _invoker.Invoke(new OnErrorDelegate(OnError), ex);

                    return;
                }

                _logServices.Trace("Notifying error...");
                Error?.Invoke(this, new ErrorEventArgs(ex));
            }
            finally
            {
                _logServices.TraceExit();
            }
        }

        private void OnError(Task t, object state)
        {
            _logServices.TraceEnter();
            try
            {
                OnError(t.Exception);
            }
            finally
            {
                _logServices.TraceExit();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            _logServices.TraceEnter();
            try
            {
                _logServices.Trace("Checking if running on UI thread...");
                if (_invoker.InvokeRequired)
                {
                    _logServices.Trace("Not running on UI thread.  Delegating to UI thread...");
                    _invoker.Invoke(new OnPropertyChangedDelegate(OnPropertyChanged), propertyName);

                    return;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            finally
            {
                _logServices.TraceExit();
            }
        }

        public void OnNext(Services.Models.Window value)
        {
            _logServices.TraceEnter();
            try
            {
                _logServices.Trace("Checking if running on UI thread...");
                if (_invoker.InvokeRequired)
                {
                    _logServices.Trace("Not running on UI thread.  Delegating to UI thread...");
                    _invoker.Invoke(new OnNextDelegate(OnNext), value);

                    return;
                }

                _logServices.Trace($"Mapping to UI layer...");
                var uiWindow = Mapper.Map<Models.Window>(value);

                _logServices.Trace($"Adding {uiWindow} to list...");
                Windows.Add(uiWindow);

                // no need to call OnProperty - the BindingList will take care of that
            }
            finally
            {
                _logServices.TraceExit();
            }
        }

        public void OnCompleted()
        {
            _logServices.TraceEnter();
            try
            {
                _logServices.Trace($"Setting {nameof(CanRefresh)} to true...");
                CanRefresh = true;
            }
            finally
            {
                _logServices.TraceExit();
            }
        }

        public Models.Window SelectedWindow {
            get => _selectedWindow;
            set {
                _logServices.TraceEnter();
                try
                {
                    // only process changes
                    if (_selectedWindow == value) return;

                    _logServices.Trace($"Saving value for {nameof(SelectedWindow)}...");
                    _selectedWindow = value;

                    _logServices.Trace($"Setting value for {nameof(CanStart)}...");
                    CanStart = _selectedWindow != null;

                    OnPropertyChanged(nameof(CanStart));
                    OnPropertyChanged();
                }
                finally
                {
                    _logServices.TraceExit();
                }
             }
        }

        public BindingList<Models.Window> Windows { get; } = new BindingList<Models.Window>();
    }
}