using Chromely;
using Chromely.Core;
using Chromely.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;

namespace Test.Chromely
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var config = DefaultConfiguration.CreateForRuntimePlatform();
            config.CefDownloadOptions.AutoDownloadWhenMissing = false;

#if DEBUG
            config.CefDownloadOptions.AutoDownloadWhenMissing = true;
#endif

            config.StartUrl = "local://dist/index.html";

            AppBuilder
                .Create()
                .UseConfig<DefaultConfiguration>(config)
                .UseApp<AngularApp>()
                .Build()
                .Run(args);
        }
    }

    public class AngularApp : ChromelyBasicApp
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddLogging(configure => configure.AddConsole());
            services.AddLogging(configure => configure.AddFile("Logs/serilog-{Date}.txt"));

            /*
            // Optional - adding custom handler
            services.AddSingleton<CefDragHandler, CustomDragHandler>();
            */

            /*
            // Optional- using config section to register IChromelyConfiguration
            // This just shows how it can be used, developers can use custom classes to override this approach
            //
            var builder = new ConfigurationBuilder()
                    .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            var config = DefaultConfiguration.CreateFromConfigSection(configuration);
            services.AddSingleton<IChromelyConfiguration>(config);
            */

            var options = new JsonSerializerOptions();
            options.ReadCommentHandling = JsonCommentHandling.Skip;
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.AllowTrailingCommas = true;

            services.AddSingleton(options);

            RegisterControllerAssembly(services, typeof(AngularApp).Assembly);
        }
    }
}
