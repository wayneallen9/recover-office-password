using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

namespace RecoverExcelPassword
{
    public class ProgressFormViewModel : INotifyPropertyChanged
    {
        #region delegates
        private delegate void OnErrorDelegate(Exception ex);
        private delegate void OnPropertyChangedDelegate(string propertyName);
        private delegate void OnTickDelegate(object sender, EventArgs e);
        #endregion

        #region events
        public event ErrorEventHandler Error;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region variables
        private CancellationTokenSource _cancellationTokenSource;
        private TimeSpan _elapsed;
        private readonly IInvoker _invoker;
        private readonly Services.ILogServices _logServices;
        private string _password;
        private readonly DateTime _startTime;
        private bool _success;
        private readonly System.Windows.Forms.Timer _timer;
        private readonly Services.IWindowServices _windowServices;
        #endregion

        public ProgressFormViewModel(
            IInvoker invoker,
            Services.ILogServices logServices,
            Services.IWindowServices windowServices)
        {
            // save dependency injections
            _invoker = invoker;
            _logServices = logServices;
            _windowServices = windowServices;

            // initialise variables
            _elapsed = TimeSpan.FromSeconds(0);
            _startTime = DateTime.Now;

            // start the timer
            _timer = new System.Windows.Forms.Timer
            {
                Interval = 1000
            };
            _timer.Tick += OnTick;
        }

        private void OnTick(object sender, EventArgs e)
        {
            _logServices.TraceEnter();
            try
            {
                _logServices.Trace("Checking if running on UI thread...");
                if (_invoker.InvokeRequired)
                {
                    _logServices.Trace("Not running on UI thread.  Delegating to UI thread...");
                    _invoker.Invoke(new OnTickDelegate(OnTick), sender, e);

                    return;
                }

                _logServices.Trace("Setting elapsed time...");
                Elapsed = DateTime.Now - _startTime;
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
            finally
            {
                _logServices.TraceExit();
            }
        }

        private string GetNextPassword(string password)
        {
            _logServices.TraceEnter();
            try
            {
                _logServices.Trace($"Converting \"{password}\" to ASCII byte array...");
                var passwordBytes = Encoding.ASCII.GetBytes(password);

                _logServices.Trace($"Incrementing \"{password}\"...");
                for (var bytePos = passwordBytes.Length; bytePos > 0; bytePos--)
                {
                    if (passwordBytes[bytePos - 1] < 126)
                    {
                        passwordBytes[bytePos - 1]++;

                        for (var spacePos = bytePos + 1; spacePos <= passwordBytes.Length; spacePos++)
                        {
                            passwordBytes[spacePos - 1] = 32;
                        }

                        return Encoding.ASCII.GetString(passwordBytes);
                    }
                }

                return new string(' ', password.Length + 1);
            }
            finally
            {
                _logServices.TraceExit();
            }
        }

        private void OnError(Exception ex)
        {
            try
            {
                if (_invoker.InvokeRequired)
                {
                    _invoker.Invoke(new OnErrorDelegate(OnError), ex);

                    return;
                }

                Error?.Invoke(this, new ErrorEventArgs(ex));
            }
            catch (Exception)
            {
                // ignore any error whilst sending the error
            }
        }

        private void OnError(Task task, object state)
        {
            try
            {
                OnError(task.Exception);
            }
            catch (Exception)
            {
                // ignore any error whilst sending the error
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (_invoker.InvokeRequired)
            {
                _invoker.Invoke(new OnPropertyChangedDelegate(OnPropertyChanged), propertyName);

                return;
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Password
        {
            get => _password;
            private set
            {
                // only process changes
                if (_password == value) return;

                // save the new value
                _password = value;

                OnPropertyChanged();
            }
        }

        public void Start(IntPtr windowHandle)
        {
            // start the timer
            _timer.Start();

            _cancellationTokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(() => StartTask(windowHandle, _cancellationTokenSource.Token))
                .ContinueWith(OnError, _cancellationTokenSource.Token, TaskContinuationOptions.OnlyOnFaulted);
        }

        private void StartTask(IntPtr hWnd, CancellationToken cancellationToken)
        {
            _logServices.TraceEnter();
            try
            {
                _logServices.Trace("Initialising password to \" \"...");
                Password = " ";

                // we keep trying until the user cancels or the password is cracked
                while (!cancellationToken.IsCancellationRequested)
                {
                    // try this password
                    if (_windowServices.TryPassword(hWnd, _password))
                    {
                        // stop the timer
                        _timer.Stop();

                        // the crack has been successful
                        Success = true;

                        // stop searching
                        break;
                    }

                    // get the next password in the sequence
                    Password = GetNextPassword(_password);
                }
            }
            finally
            {
                _logServices.TraceExit();
            }
        }

        public void Stop()
        {
            // cancel the async task
            _cancellationTokenSource?.Cancel();

            // stop the timer
            _timer.Stop();
        }

        public TimeSpan Elapsed
        {
            get => _elapsed;
            private set
            {
                // only process changes
                if (_elapsed == value) return;

                // save the new value
                _elapsed = value;

                OnPropertyChanged();
            }
        }

        public bool Success
        {
            get => _success;
            private set
            {
                // only process changes
                if (_success == value) return;

                // save the new value
                _success = value;

                OnPropertyChanged();
            }
        }
    }
}