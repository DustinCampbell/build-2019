using System;

namespace TollCollectorLib
{
    public enum LogLevel
    {
        Info,
        Warning,
        Error
    }

    public interface ILogger
    {
        void SendMessage(string message, LogLevel logLevel = LogLevel.Info);
        void SendError(Exception ex) => SendMessage(ex.Message, LogLevel.Error);
    }
}
