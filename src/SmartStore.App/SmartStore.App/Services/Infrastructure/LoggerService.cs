using System;
using System.Collections.Generic;
using SmartStore.App.Abstractions.Device;
using SmartStore.App.Abstractions.Infrastructure;

namespace SmartStore.App.Services.Infrastructure
{
    public class LoggerService : ILoggerService
    {
        private readonly ISettingsService _settingsService;

        public LoggerService(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public void Trace(string eventName, Dictionary<string, string> extraProperties = null)
        {
            try
            {
                var currentSession = _settingsService;
                if (currentSession == null)
                    throw new Exception("Session not started to Trace !");

                var properties = extraProperties ?? new Dictionary<string, string>();
                properties.Add("Login Id", currentSession.AuthIdToken);
                properties.Add("Time", DateTimeOffset.Now.ToString());
                properties.Add("UserId", currentSession.AuthIdToken);

                //Analytics.TrackEvent(eventName, properties);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message, ex.InnerException);
            }

        }

        public void Error(Exception exception, Dictionary<string, string> extraLogs = null)
        {
            try
            {
                var properties = extraLogs ?? new Dictionary<string, string>();
                var currentSession = _settingsService;
                if (currentSession != null)
                {
                    properties.Add("UserId", currentSession.AuthIdToken);
                    properties.Add("Login Id", currentSession.AuthAccessToken);
                }

                properties.Add("StackTrace", exception.StackTrace ?? string.Empty);
                properties.Add("InnerException", exception.InnerException?.ToString() ?? string.Empty);
                properties.Add("Exception Message", exception.Message ?? string.Empty);
                properties.Add("Exception Source", exception.Source ?? string.Empty);
                properties.Add("Time", DateTimeOffset.Now.ToString());

                //Crashes.TrackError(exception, properties);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message, ex.InnerException);
            }
        }
    }
}