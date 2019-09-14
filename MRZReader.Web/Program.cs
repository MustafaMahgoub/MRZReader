using System;
using System.Xml;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;

namespace MRZReader.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
                logger.Debug($"|| MRZ APP STARTED");
                logger.Debug($"|| Deplyed on : {DateTime.Now}");
                logger.Debug("||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");

                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                //Let nlog catch setup errors
                logger.Error($"KO :Exception: {e.Message}");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .UseStartup<Startup>().UseNLog();
    }
}
