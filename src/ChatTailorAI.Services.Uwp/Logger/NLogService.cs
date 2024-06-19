using System;
using NLog;
using NLog.Config;
using NLog.Targets;
using ChatTailorAI.Shared.Services.Common;

namespace ChatTailorAI.Services.Uwp
{
    public class NLogService : ILoggerService
    {
        public NLogService()
        {
            ConfigureNLog();
        }

        private void ConfigureNLog()
        {
            var config = new LoggingConfiguration();
            var logFile = new FileTarget("logfile")
            {
                FileName = $"{Windows.Storage.ApplicationData.Current.LocalFolder.Path}\\logs\\log_${{date:format=yyyy-MM-dd}}.txt",
                Layout = "[${longdate}] [${level}] ${message} ${exception}"
            };

            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logFile);
            LogManager.Configuration = config;
        }

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public void Debug(string message) => Logger.Debug(message);
        public void Info(string message) => Logger.Info(message);
        public void Warn(string message) => Logger.Warn(message);
        public void Error(string message) => Logger.Error(message);
        public void Error(Exception ex, string message) => Logger.Error(ex, message);
        public void Fatal(string message) => Logger.Fatal(message);
        public void Fatal(Exception ex, string message) => Logger.Fatal(ex, message);
    }
}