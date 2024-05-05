using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.TestHost;

namespace ServiceBricks.Xunit
{
    public class SystemManager : ISystemManager
    {
        public IServiceProvider ServiceProvider { get; set; }
        public IConfiguration Configuration { get; set; }
        public TestServer TestServer { get; set; }

        private CancellationTokenSource CancellationTokenSource;
        private SemaphoreSlim _signal = new SemaphoreSlim(0);

        public virtual IWebHostBuilder CreateWebHostBuilder(Type startupType)
        {
            // Wipeout previous run
            BusinessRuleRegistry.Instance = new BusinessRuleRegistry();

            return new WebHostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddAppSettingsConfig();
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                })
                .UseEnvironment("Development")
                .UseStartup(startupType);
        }

        public virtual void StopSystem()
        {
            CancellationTokenSource?.Cancel();
            TestServer.Dispose();
        }

        public virtual void StartSystem(Type startupType)
        {
            ServiceBricks.Startup.ConfigureCompleteEvent += Startup_ConfigureCompleteEvent;

            // Create host builder
            var webHostBuilder = CreateWebHostBuilder(startupType);

            TestServer = new Microsoft.AspNetCore.TestHost.TestServer(webHostBuilder);
            ServiceProvider = TestServer.Services;
            Configuration = ServiceBricks.Startup.Configuration;
            CancellationTokenSource = new CancellationTokenSource();

            _signal.Wait(CancellationTokenSource.Token);
        }

        public virtual void Startup_ConfigureCompleteEvent(IApplicationBuilder app)
        {
            _signal.Release();
        }
    }
}