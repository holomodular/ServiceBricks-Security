using ServiceBricks.Security.EntityFrameworkCore;
using System.Reflection;

namespace ServiceBricks.Security.Sqlite
{
    public class SecuritySqliteModule : IModule
    {
        public SecuritySqliteModule()
        {
            AutomapperAssemblies = new List<Assembly>()
            {
                typeof(SecuritySqliteModule).Assembly
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