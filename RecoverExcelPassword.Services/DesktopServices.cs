using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RecoverExcelPassword.Services
{
    public class DesktopActions : IDesktopActions
    {
        #region winapi
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern IntPtr WindowFromPoint(Point p);
        #endregion

        #region variables
        private readonly IKeyboardMouseEvents _keyboardMouseEvents;
        private readonly ILogServices _logServices;
        private readonly IList<IDesktopActionsObserver> _observers;
        #endregion

        public DesktopActions(
            ILogServices logServices)
        {
            // save dependency injections
            _logServices = logServices;

            // initialise variables
            _keyboardMouseEvents = Hook.GlobalEvents();
            _observers = new List<IDesktopActionsObserver>();
        }

        public void StartWatching()
        {
            _logServices.TraceEnter();
            try
            {
                _logServices.Trace("Adding event handlers...");
                _keyboardMouseEvents.MouseClick += MouseUpExtHandler;
            }
            finally
            {
                _logServices.TraceExit();
            }
        }

        private void MouseUpExtHandler(object sender, MouseEventArgs e)
        {
            _logServices.TraceEnter();
            try
            {
                _logServices.Trace("Getting clicked window...");
                var clickedHandle = WindowFromPoint(e.Location);

                _logServices.Trace($"Notifying {_observers.Count} observers...");
                foreach (var observer in _observers) observer.OnClick(clickedHandle);
            }
            finally
            {
                _logServices.TraceExit();
            }
        }

        public void StopWatching()
        {
            _logServices.TraceEnter();
            try
            {
                _logServices.Trace("Removing event handlers...");
                _keyboardMouseEvents.MouseUpExt -= MouseUpExtHandler;
            }
            finally
            {
                _logServices.TraceExit();
            }
        }

        public void Subscribe(IDesktopActionsObserver observer)
        {
            _logServices.TraceEnter();
            try
            {
                _logServices.Trace("Checking if observer is already subscribed...");
                if (!_observers.Contains(observer))
                {
                    _logServices.Trace("Observer is not already subscribed.  Subscribing...");
                    _observers.Add(observer);
                }
            }
            finally
            {
                _logServices.TraceExit();
            }
        }
    }
}