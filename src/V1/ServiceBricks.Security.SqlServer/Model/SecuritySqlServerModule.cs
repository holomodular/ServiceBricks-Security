using ServiceBricks.Security.EntityFrameworkCore;
using System.Reflection;

namespace ServiceBricks.Security.SqlServer
{
    public class SecuritySqlServerModule : IModule
    {
        public SecuritySqlServerModule()
        {
            AdminHtml = string.Empty;
            Name = "Security SqlServer Module";
            Description = @"The Security EntityFrameworkCore Module implements the EntityFrameworkCore provider.";
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