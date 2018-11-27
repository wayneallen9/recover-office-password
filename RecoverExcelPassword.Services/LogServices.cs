using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace RecoverExcelPassword.Services
{
    public class LogServices : ILogServices
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly Stack<Stopwatch> _stopWatches;

        private int _indent = 0;

        public LogServices()
        {
            _stopWatches = new Stack<Stopwatch>();
        }

        public void Error(Exception ex)
        {
            try
            {
                _logger.Error(ex);
            }
            catch (Exception)
            {
                // ignore any exceptions when logging
            }
        }

        public void Trace(string message)
        {
            try
            {
                // log the message
                _logger.Trace($"{new String('\t', _indent)}{message}");
            }
            catch (Exception)
            {
                // ignore any exceptions when logging
            }
        }

        public void TraceEnter()
        {
            try
            {
                // log the message
                _logger.Trace($"{new String('\t', _indent++)}Enter");

                // start the stopwatch for this method
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                // push it to the stack for the matching exit
                _stopWatches.Push(stopWatch);
            }
            catch (Exception)
            {
                // ignore any exceptions when logging
            }
        }

        public void TraceExit()
        {
            try
            {
                // get the stopwatch for this method
                var stopWatch = _stopWatches.Pop();

                _logger.Trace($"{new String('\t', --_indent)}Exit - {stopWatch.ElapsedMilliseconds}ms");
            }
            catch (Exception)
            {
                // ignore any exceptions when logging
            }
        }

        public void Warn(string message)
        {
            try
            {
                _logger.Warn($"{message}");
            }
            catch (Exception)
            {
                // ignore any exceptions when logging
            }
        }
    }
}