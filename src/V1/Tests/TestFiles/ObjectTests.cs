using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceBricks.Security;

namespace ServiceBricks.Xunit
{
    [Collection(Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class ObjectTests
    {
        public virtual ISystemManager SystemManager { get; set; }

        public ObjectTests()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupInMemory));
        }

        [Fact]
        public virtual Task DtoTests()
        {
            var loggerFactory = SystemManager.ServiceProvider.GetRequiredService<ILoggerFactory>();
            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task ModuleTests()
        {
            SecurityModule module = new SecurityModule();

            var dep = module.DependentModules;
            var au = module.AutomapperAssemblies;
            var vi = module.ViewAssemblies;

            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task AddSecurityClientTests()
        {
            IServiceCollection services = new ServiceCollection();
            IConfiguration config = new ConfigurationBuilder().Build();

            services.AddServiceBricksSecurityClient(config);
            return Task.CompletedTask;
        }

        [Fact]
        public virtual Task AddSecurityClientForServiceTests()
        {
            IServiceCollection services = new ServiceCollection();
            IConfiguration config = new ConfigurationBuilder().Build();

            services.AddServiceBricksSecurityClientForService(config);
            return Task.CompletedTask;
        }
    }
}