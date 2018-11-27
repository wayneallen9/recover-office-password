using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace RecoverExcelPassword.Services
{
    public class WindowServices : IWindowServices, System.IObserver<Models.Window>
    {
        #region constants
        private const int MK_LBUTTON = 0x0001;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_SETTEXT = 0x000c;
        #endregion

        #region delegates
        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
        #endregion

        #region winapi
        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "PostMessageA")]
        public static extern bool PostMessage(IntPtr hWnd, int uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int uMsg, IntPtr wParam, string lParam);
        #endregion

        #region variables
        private readonly ILogServices _logServices;
        private readonly IWindowFinder _windowFinder;
        private Exception _windowFinderException;
        #endregion

        public WindowServices(
            ILogServices logServices,
            IWindowFinder windowFinder)
        {
            // save dependency injections
            _logServices = logServices;
            _windowFinder = windowFinder;

            // subscribe to found windows
            _windowFinder.Subscribe(this);
        }

        public IntPtr MakeLParam(int LoWord, int HiWord)
        {
            return (IntPtr)((HiWord << 16) | (LoWord & 0xffff));
        }

        public bool TryPassword(IntPtr windowHandle, string password)
        {
            _logServices.TraceEnter();
            try
            {
                // find the password window
                var passwordHandle = FindWindowEx(windowHandle, IntPtr.Zero, "EDIT", null);
                if (passwordHandle == IntPtr.Zero) throw new Win32Exception(Marshal.GetLastWin32Error());

                // send the password to the text box
                SendMessage(passwordHandle, WM_SETTEXT, IntPtr.Zero, password);

                // find the OK button
                var okHandle = FindWindowEx(windowHandle, IntPtr.Zero, "BUTTON", "OK");

                // click the OK button
                ClickButton(okHandle, 10, 10);

                // give Excel time to response
                System.Threading.Thread.Sleep(50);

                // has the window disappeared?
                if (!IsWindowVisible(windowHandle)) return true;

                // get the process id for this window
                GetWindowThreadProcessId(windowHandle, out int processId);

                // click the OK button on the new window
                _windowFinderException = null;
                foreach (ProcessThread thread in Process.GetProcessById(processId).Threads)
                {
                    _windowFinder.Find(thread.Id);
                }

                // if an Exception was thrown, bubble it up
                if (_windowFinderException != null) throw _windowFinderException;

                // give the window time to close
                System.Threading.Thread.Sleep(10);

                return false;
            }
            finally
            {
                _logServices.TraceExit();
            }
        }

        private void ClickButton(IntPtr buttonHandle, int x, int y)
        {
            // create the param
            var param = MakeLParam(x, y);

            // click the button
            PostMessage(buttonHandle, WM_LBUTTONDOWN, IntPtr.Zero, param);
            PostMessage(buttonHandle, WM_LBUTTONUP, IntPtr.Zero, param);
        }

        public void OnNext(Models.Window value)
        {
            _logServices.TraceEnter();
            try
            {
                _logServices.Trace($"Checking if {value} is the Project Locked window...");
                if (value.Title != "Project Locked")
                {
                    _logServices.Trace($"\"{value}\" is not the Project Locked window.  Exiting...");
                    return;
                }

                _logServices.Trace($"Clicking OK button on {value.Handle}...");
                var okHandle = FindWindowEx(value.Handle, IntPtr.Zero, "BUTTON", "OK");
                ClickButton(okHandle, 10, 10);
            }
            finally
            {
                _logServices.TraceExit();
            }
        }

        public void OnError(Exception error)
        {
            _logServices.TraceEnter();
            try
            {
                _windowFinderException = error;
            }
            finally
            {
                _logServices.TraceExit();
            }
        }

        public void OnCompleted()
        {
            // no special action required
        }
    }
}