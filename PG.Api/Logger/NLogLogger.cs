using System;
using PG.Common;

namespace PG.Api
{
    public class NLogLogger : ILogger
    {
        private readonly NLog.Logger _logger;

        public NLogLogger()
        {
            _logger = NLog.LogManager.GetLogger("default");
        }

        public NLogLogger(string name)
        {
            _logger = NLog.LogManager.GetLogger(name);
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Debug(string message, Exception ex)
        {
            _logger.Debug(ex, message);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Error(string message, Exception ex)
        {
            _logger.Error(ex, message);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Info(string message, Exception ex)
        {
            _logger.Info(ex, message);
        }

        public void Warning(string message)
        {
            _logger.Warn(message);
        }

        public void Warning(string message, Exception ex)
        {
            _logger.Warn(ex, message);
        }
    }
}
