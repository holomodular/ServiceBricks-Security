using ServiceBricks.Security.EntityFrameworkCore;
using System.Reflection;

namespace ServiceBricks.Security.Sqlite
{
    public class SecuritySqliteModule : IModule
    {
        public SecuritySqliteModule()
        {
            AdminHtml = string.Empty;
            Name = "Security Sqlite Module";
            Description = @"The Security EntityFrameworkCore Module implements the EntityFrameworkCore provider.";
            AutomapperAssemblies = new List<Assembly>()
            {
                typeof(SecuritySqliteModule).Assembly
            };
            DependentModules = new List<IModule>()
            {
                new SecurityEntityFrameworkCoreModule()
            };
        }

        public string Name { get; }
        public string Description { get; }
        public string AdminHtml { get; }
        public List<Assembly> AutomapperAssemblies { get; }
        public List<Assembly> ViewAssemblies { get; }

        public List<IModule> DependentModules { get; }
    }
}