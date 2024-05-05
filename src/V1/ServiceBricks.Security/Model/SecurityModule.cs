using System.Collections.Generic;
using System.Reflection;

namespace ServiceBricks.Security
{
    public class SecurityModule : IModule
    {
        public SecurityModule()
        {
            AdminHtml = string.Empty;
            Name = "Security Brick";
            Description = @"The Security Brick is responsible for application security.";
        }

        public string Name { get; }
        public string Description { get; }
        public string AdminHtml { get; }
        public List<Assembly> AutomapperAssemblies { get; }
        public List<Assembly> ViewAssemblies { get; }
        public List<IModule> DependentModules { get; }
    }
}