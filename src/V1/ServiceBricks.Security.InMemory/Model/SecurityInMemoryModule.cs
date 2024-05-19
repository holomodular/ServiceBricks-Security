using System.Reflection;
using ServiceBricks.Security.EntityFrameworkCore;

namespace ServiceBricks.Security.InMemory
{
    public class SecurityInMemoryModule : IModule
    {
        public SecurityInMemoryModule()
        {
            AutomapperAssemblies = new List<Assembly>()
            {
                typeof(SecurityInMemoryModule).Assembly
            };
            DependentModules = new List<IModule>()
            {
                new SecurityEntityFrameworkCoreModule()
            };
        }

        public List<Assembly> AutomapperAssemblies { get; }
        public List<Assembly> ViewAssemblies { get; }

        public List<IModule> DependentModules { get; }
    }
}