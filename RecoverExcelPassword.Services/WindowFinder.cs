using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace RecoverExcelPassword.Services
{
    public class WindowFinder : IWindowFinder
    {
        #region delegates
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
        #endregion

        #region winapi
        [DllImport("user32.dll", SetLastError =true)]
        [return:MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError =true)]
        [return:MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumThreadWindows(int dwThreadId, EnumWindowsProc lpfn, IntPtr lParam);

        [DllImport("user32", SetLastError =true, CharSet =CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hwnd, StringBuilder buffer, int len);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        #endregion

        #region variables
        private readonly ILogServices _logServices;
        private readonly IList<System.IObserver<Models.Window>> _observers;
        #endregion

        public WindowFinder(
            ILogServices logServices)
        {
            // save dependency injections
            _logServices = logServices;

            // initialise variables
            _observers = new List<System.IObserver<Models.Window>>();
        }

        private bool FindWindowHandler(IntPtr hWnd, IntPtr lParam)
        {
            _logServices.TraceEnter();
            try
            {
                // get the text for this window
                var windowText = GetWindowText(hWnd);

                // save the details of the found window
                OnUpdate(new Models.Window
                {
                    Handle = hWnd,
                    Title = windowText
                });

                return true;
            }
            catch (Exception ex)
            {
                OnError(ex);

                return true;
            }
            finally
            {
                _logServices.TraceExit();
            }
        }

        private bool ListWindowsHandler(IntPtr hWnd, IntPtr lParam)
        {
            _logServices.TraceEnter();
            try
            {
                _logServices.Trace($"Getting process associated with window {hWnd}...");
                var thread = GetWindowThreadProcessId(hWnd, out uint processId);
                var process = Process.GetProcessById((int)processId);

                _logServices.Trace($"Checking if \"{process.ProcessName}\" is an Office process...");
                switch (process.ProcessName.ToLower())
                {
                    case "excel":
                    case "winword":

                        break;
                    default:
                        _logServices.Trace($"\"{process.ProcessName}\" is not an Office process.  Returning...");

                        return true;
                }

                // get the text for this window
                var windowText = GetWindowText(hWnd);

                _logServices.Trace($"Checking if \"{windowText}\" is a password window...");
                if (!windowText.EndsWith(" Password"))
                {
                    _logServices.Trace($"\"{windowText}\" is not a password window.  Returning...");
                    return true;
                }

                _logServices.Trace($"Notifying window...");
                var window = new Models.Window
                {
                    Handle = hWnd,
                    Title = windowText
                };
                OnUpdate(window);

                return true;
            }
            catch (Exception ex)
            {
                OnError(ex);

                return true;
            }
            finally
            {
                _logServices.TraceExit();
            }
        }

        public void Find()
        {
            _logServices.TraceEnter();
            try
            {
                _logServices.Trace("Enumerating windows...");
                var enumWindowsCallback = new EnumWindowsProc(ListWindowsHandler);
                EnumWindows(enumWindowsCallback, IntPtr.Zero);

                OnCompleted();
            }
            finally
            {
                _logServices.TraceExit();
            }
        }

        public void Find(int processId)
        {
            _logServices.TraceEnter();
            try
            {
                _logServices.Trace($"Enumerating all windows for process {processId}...");
                var findWindowHandler = new EnumWindowsProc(FindWindowHandler);
                EnumThreadWindows(processId, findWindowHandler, IntPtr.Zero);

                OnCompleted();
            }
            finally
            {
                _logServices.TraceExit();
            }
        }

        private string GetWindowText(IntPtr hWnd)
        {
            _logServices.TraceEnter();
            try
            {
                _logServices.Trace($"Getting text length of window {hWnd}");
                var windowTextLength = GetWindowTextLength(hWnd);
                if (windowTextLength < 0)
                {
                    _logServices.Trace("An error occurred getting the length of the window text.  Throwing exception...");
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                else if (windowTextLength == 0)
                {
                    _logServices.Trace("Window text length is 0.  Returning...");
                    return string.Empty;
                }

                _logServices.Trace($"Getting text of window {hWnd}...");
                var windowTextBuilder = new StringBuilder(windowTextLength + 1);
                var result = GetWindowText(hWnd, windowTextBuilder, windowTextBuilder.Capacity);
                if (result < 0)
                {
                    _logServices.Trace("An error occurred getting the window text.  Throwing exception...");
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                return windowTextBuilder.ToString();
            }
            finally
            {
                _logServices.TraceExit();
            }
        }

        protected void OnCompleted()
        {
            _logServices.TraceEnter();
            try
            {
                _logServices.Trace($"Notifying {_observers.Count} observers...");
                foreach (var observer in _observers) observer.OnCompleted();
            }
            finally
            {
                _logServices.TraceExit();
            }
        }

        protected void OnError(Exception ex)
        {
            _logServices.TraceEnter();
            try
            {
                _logServices.Trace($"Notifying {_observers.Count} observers...");
                foreach (var observer in _observers) observer.OnError(ex);
            }
            finally
            {
                _logServices.TraceExit();
            }
        }

        protected void OnUpdate(Models.Window window)
        {
            _logServices.TraceEnter();
            try
            {
                _logServices.Trace($"Notifying {_observers.Count} observers...");
                foreach (var observer in _observers) observer.OnNext(window);
            }
            finally
            {
                _logServices.TraceExit();
            }
        }

        public IDisposable Subscribe(System.IObserver<Models.Window> observer)
        {
            _logServices.TraceEnter();
            try
            {
                _logServices.Trace("Checking if observer is already subscribed...");
                if (!_observers.Contains(observer))
                {
                    _logServices.Trace("Observer is not subscribed.  Subscribing...");
                    _observers.Add(observer);
                }

                return new Unsubscriber<Models.Window>(_observers, observer);
            }
            finally
            {
                _logServices.TraceExit();
            }
        }
    }
}