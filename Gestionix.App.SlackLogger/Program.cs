using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SlackLogger;


namespace Gestionix.App.SlackLogger
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
                 WebHost.CreateDefaultBuilder(args)
                     .ConfigureLogging((hostingContext, logging) =>
                     {
                         logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                         logging.AddSlack(options =>
                         {
                             options.WebhookUrl = "https://hooks.slack.com/services/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";
                             options.LogLevel = LogLevel.Information;
                             options.NotificationLevel = LogLevel.None;
                             options.Channel = "#slacklogger";
                         });
                         //logging.AddDebug();

                     })
                     .UseStartup<Startup>()
                     .Build();
    }
}
