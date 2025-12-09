using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.Xunit
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class MappingTest
    {
        public virtual ISystemManager SystemManager { get; set; }

        public MappingTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupInMemory));
        }

        [Fact]
        public virtual Task ValidateMapperConfiguration()
        {
            var mapper = SystemManager.ServiceProvider.GetRequiredService<IMapper>();

            return Task.CompletedTask;
        }
    }
}