using System.Reflection;
using ServiceBricks.Security.EntityFrameworkCore;

namespace ServiceBricks.Security.InMemory
{
    public class SecurityInMemoryModule : IModule
    {
        public SecurityInMemoryModule()
        {
            AdminHtml = string.Empty;
            Name = "Security InMemory Module";
            Description = @"The Security EntityFrameworkCore Module implements the EntityFrameworkCore provider.";
            AutomapperAssemblies = new List<Assembly>()
            {
                typeof(SecurityInMemoryModule).Assembly
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