using System;
using System.Collections.Generic;

namespace SmartStore.App.Abstractions.Infrastructure
{
    public interface ILoggerService
    {
        void Trace(string eventName, Dictionary<string, string> extraProperties = null);
        void Error(Exception exception, Dictionary<string, string> extraLogs = null);
    }
}