using ServiceBricks.Security.EntityFrameworkCore;
using System.Reflection;

namespace ServiceBricks.Security.InMemory
{
    /// <summary>
    /// The module definition for the ServiceBricks Security InMemory module.
    /// </summary>
    public partial class SecurityInMemoryModule : ServiceBricks.Module
    {
        public static SecurityInMemoryModule Instance = new SecurityInMemoryModule();

        /// <summary>
        /// Constructor.
        /// </summary>
        public SecurityInMemoryModule()
        {
            AutomapperAssemblies = new List<Assembly>()
            {
                typeof(SecurityInMemoryModule).Assembly
            };
            DependentModules = new List<IModule>()
            {
                new SecurityEntityFrameworkCoreModule()
            };
        }
    }
}