
using NLog;
namespace Chat.DesktopClient2.Logging
{
    class Log
    {
        Logger _logger;
        public Log()
        {

            var config = new NLog.Config.LoggingConfiguration();
            var logfile = new NLog.Targets.FileTarget("logfile")
            {
                FileName = "logfile.txt"
            };

            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            LogManager.Configuration = config;

            var logger = LogManager.GetCurrentClassLogger();
            _logger = logger;

        }

        public void Info(string a)
        {
            _logger.Info(a);
        }
        public void Debug(string a)
        {
            _logger.Debug(a);
        }
        public void Warn(string a)
        {
            _logger.Warn(a);
        }

        public void Trace(string a)
        {
            _logger.Trace(a);
        }

        public void Error(string a)
        {
            _logger.Error(a);
        }

        public void Fatal(string a)
        {
            _logger.Fatal(a);
        }

    }
}