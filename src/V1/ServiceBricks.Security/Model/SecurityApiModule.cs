using System.Reflection;

namespace ServiceBricks.Security
{
    public class SecurityApiModule : IModule
    {
        public SecurityApiModule()
        {
            AdminHtml = string.Empty;
            Name = "Security API Brick";
            Description = @"The Security API Brick contains clients, data transfer objects, events and interfaces.";
        }

        public string Name { get; }
        public string Description { get; }
        public string AdminHtml { get; }
        public List<Assembly> AutomapperAssemblies { get; }
        public List<Assembly> ViewAssemblies { get; }
        public List<IModule> DependentModules { get; }
    }
}