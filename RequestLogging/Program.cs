using System.IO;
using System.Threading;
using log4net.Config;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using RequestLogging.Logging;

[assembly: XmlConfigurator(ConfigFile = "LogConfiguration.log4net", Watch = true)]

namespace RequestLogging
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LogSupport.LogLevelSetters = new LogLevelRulesCompiler().Compile(
                new LogLevelRulesFileReader().ReadFile("LogLevelRules.json")
            );

            var watcher = new FileSystemWatcher
            {
                Path = Directory.GetCurrentDirectory(),
                Filter = "*.json",
                NotifyFilter = NotifyFilters.LastWrite
            };
            watcher.Changed += (sender, eventArgs) =>
            {
                // Wait while an application modifying the file release lock.
                Thread.Sleep(1000);

                LogSupport.LogLevelSetters = new LogLevelRulesCompiler().Compile(
                    new LogLevelRulesFileReader().ReadFile("LogLevelRules.json")
                );
            };
            watcher.EnableRaisingEvents = true;

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
