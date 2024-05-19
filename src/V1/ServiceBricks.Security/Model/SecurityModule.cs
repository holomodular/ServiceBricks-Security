using System.Collections.Generic;
using System.Reflection;

namespace ServiceBricks.Security
{
    public class SecurityModule : IModule
    {
        public SecurityModule()
        {
        }

        public List<Assembly> AutomapperAssemblies { get; }
        public List<Assembly> ViewAssemblies { get; }
        public List<IModule> DependentModules { get; }
    }
}