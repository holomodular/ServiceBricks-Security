using System.Collections.Generic;
using System.Reflection;

namespace ServiceBricks.Security.Member
{
    public class SecurityMemberModule : IModule
    {
        public SecurityMemberModule()
        {
        }

        public List<Assembly> AutomapperAssemblies { get; }
        public List<Assembly> ViewAssemblies { get; }
        public List<IModule> DependentModules { get; }
    }
}