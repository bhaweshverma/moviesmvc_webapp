using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MoviesMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging => {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddDebug();
                    //logging.AddEventLog();
                    logging.AddEventLog(eventLogSettings  => {
                        eventLogSettings.SourceName = "Movies MVC Web App";
                        //options.MachineName = "local machine";
                    });
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
