using ApiGateway;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Ocelot.DependencyInjection;
using Ocelot.Provider.Polly;

namespace AgentPortalApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config
                        .AddJsonFile("Ocelot.json", false, true)
                        .AddJsonFile($"Ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json", true,
                            true);
                })
                .ConfigureWebHostDefaults(webbuilder =>
                {
                    webbuilder.UseStartup<Startup>();
                    webbuilder.ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.SetMinimumLevel(LogLevel.Trace);
                    });
                    webbuilder.UseNLog();
                })
                .ConfigureServices(
                    s => s.AddOcelot().AddPolly()
                );
    }
}
