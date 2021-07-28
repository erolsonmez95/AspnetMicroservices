using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OcelotApiGw
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            //added to set which json used to configure (either ocelot.Development or ocelot.Local)
            .ConfigureAppConfiguration((hostingContext,config)=> {
                config.AddJsonFile($"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json",true,true);
            })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            .ConfigureLogging((hostingContext, loggingBuilder)=>{
               //logging needed to track ocelot gateway
                loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });
    }
}
