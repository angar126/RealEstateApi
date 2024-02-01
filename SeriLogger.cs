using System;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using System.IO;

namespace CommonLogger
{
    public class SeriLogger
    {
        public static Action<HostBuilderContext, LoggerConfiguration> Configure = (context, configuration) =>
        {
            var logDirectory = Path.Combine(context.HostingEnvironment.ContentRootPath, "Logs");
            var logFilePath = Path.Combine(logDirectory, "log-.txt");

            EnsureDirectory(logDirectory);

            configuration
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
                .ReadFrom.Configuration(context.Configuration);
        };




        public static void EnsureDirectory(string logFilePath)
        {
            var logDirectory = Path.GetDirectoryName(logFilePath);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }
    }
}
