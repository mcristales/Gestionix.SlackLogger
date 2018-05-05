﻿using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SlackLogger
{
    public class SlackLoggerOptions
    {
        public string Channel { get; set; }
        public string ApplicationName { get; set; }
        public string WebhookUrl { get; set; }
        public string EnvironmentName { get; set; }
        public Func<string, string> SanitizeOutputFunction { get; set; }
        private LogLevel? _logLevel;
        public LogLevel LogLevel 
        {
            get => _logLevel ?? Microsoft.Extensions.Logging.LogLevel.Warning;
            set => _logLevel = value;
        }
        private LogLevel? _notificationLevel;
        public LogLevel NotificationLevel
        {
            get => _notificationLevel ?? Microsoft.Extensions.Logging.LogLevel.Error;
            set => _notificationLevel = value;
        }

        public void Merge(IConfiguration configuration)
        {
            if (configuration != null)
            {
                if (configuration["ApplicationName"] != null)
                {
                    ApplicationName = configuration["ApplicationName"];
                }
                if (configuration["Channel"] != null)
                {
                    Channel = configuration["Channel"];
                }
                if (configuration["WebhookUrl"] != null)
                {
                    WebhookUrl = configuration["WebhookUrl"];
                }
                if (configuration["Environment"] != null)
                {
                    EnvironmentName = configuration["Environment"];
                }
                if (configuration["LogLevel"] != null)
                {
                    LogLevel = ParseLogLevel(configuration, "LogLevel");
                }
                if (configuration["NotificationLevel"] != null)
                {
                    NotificationLevel = ParseLogLevel(configuration, "NotificationLevel");
                }
            }
        }

        private static LogLevel ParseLogLevel(IConfiguration config, string key)
        {
            try
            {
                return (LogLevel)Enum.Parse(typeof(LogLevel), config[key]);
            }
            catch (Exception e)
            {
                throw new Exception(
                    $"Can't map the config value {key} to a LogLevel. Allowed values [{string.Join(", ", Enum.GetValues(typeof(LogLevel)).Cast<LogLevel>())}]", e);
            }
        }

        public void ValidateWebookUrl()
        {
            if (string.IsNullOrWhiteSpace(WebhookUrl))
            {
                throw new ArgumentException("WebhookUrl must be set");
            }
            Uri uriResult;
            if (!(Uri.TryCreate(WebhookUrl, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == "http" || uriResult.Scheme == "https")))
            {
                throw new ArgumentException($"Invalid WebhookUrl: {WebhookUrl}");
            }

        }
    }
}
