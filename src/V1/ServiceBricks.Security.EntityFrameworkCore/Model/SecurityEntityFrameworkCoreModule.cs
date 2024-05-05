using System.Reflection;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    public class SecurityEntityFrameworkCoreModule : IModule
    {
        public SecurityEntityFrameworkCoreModule()
        {
            AdminHtml = string.Empty;
            Name = "Security EntityFrameworkCore Module";
            Description = @"The Security EntityFrameworkCore Module implements the EntityFrameworkCore provider.";
            AutomapperAssemblies = new List<Assembly>()
            {
                typeof(SecurityEntityFrameworkCoreModule).Assembly
            };
            DependentModules = new List<IModule>()
            {
                new SecurityModule()
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