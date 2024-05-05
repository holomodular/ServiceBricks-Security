using ServiceBricks.Security.EntityFrameworkCore;
using System.Reflection;

namespace ServiceBricks.Security.Cosmos
{
    public class SecurityCosmosModule : IModule
    {
        public SecurityCosmosModule()
        {
            AutomapperAssemblies = new List<Assembly>()
            {
                typeof(SecurityCosmosModule).Assembly
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