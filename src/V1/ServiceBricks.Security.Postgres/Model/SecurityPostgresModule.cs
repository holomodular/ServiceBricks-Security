using ServiceBricks.Security.EntityFrameworkCore;
using System.Reflection;

namespace ServiceBricks.Security.Postgres
{
    public class SecurityPostgresModule : IModule
    {
        public SecurityPostgresModule()
        {
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