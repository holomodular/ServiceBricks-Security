using System.Reflection;

namespace ServiceBricks.Security.MongoDb
{
    public class SecurityMongoDbModule : IModule
    {
        public SecurityMongoDbModule()
        {
            AutomapperAssemblies = new List<Assembly>()
            {
                typeof(SecurityMongoDbModule).Assembly
            };
            DependentModules = new List<IModule>()
            {
                new SecurityModule()
            };
        }

        public List<Assembly> AutomapperAssemblies { get; }
        public List<Assembly> ViewAssemblies { get; }
        public List<IModule> DependentModules { get; }
    }
}