using System.Reflection;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    public class SecurityEntityFrameworkCoreModule : IModule
    {
        public SecurityEntityFrameworkCoreModule()
        {
            AutomapperAssemblies = new List<Assembly>()
            {
                typeof(SecurityEntityFrameworkCoreModule).Assembly
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