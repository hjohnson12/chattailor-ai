using System;

namespace ChatTailorAI.Shared.Services.Common
{
    public interface ILoggerService
    {
        void Debug(string message);
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        void Error(Exception ex, string message);
        void Fatal(string message);
        void Fatal(Exception ex, string message);
    }
}