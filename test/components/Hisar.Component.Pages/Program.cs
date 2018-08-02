using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NetCoreStack.Hisar;
using System.IO;

namespace Hisar.Component.Pages
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddCommandLine(args).Build();

            var host = new WebHostBuilder()
                .UseConfiguration(configuration)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<DefaultHisarStartup<Startup>>()
                //.UseUrls("http://localhost:5050")
                .Build();

            host.Run();
        }
    }
}
