using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WorkerService1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RunAspNetCoreApp(args);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                });

        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls("http://localhost:5123")
                    .UseStartup<Startup>();
            });

        public static Task RunAspNetCoreApp(string[] args)
        {
            var runningAsService = !(Debugger.IsAttached || args.Contains("--console"));

            if (runningAsService)
            {
                // Change the current working directory from System32 to the app dir for routing purposes.
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                Directory.SetCurrentDirectory(pathToContentRoot);
            }

            var builder = CreateWebHostBuilder(args);
            var host = builder.Build();

            return host.RunAsync();
        }
    }
}
