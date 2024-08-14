using ServiceBricks.Security.EntityFrameworkCore;
using System.Reflection;

namespace ServiceBricks.Security.InMemory
{
    /// <summary>
    /// The module definition for the ServiceBricks Security InMemory module.
    /// </summary>
    public partial class SecurityInMemoryModule : IModule
    {
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

        /// <summary>
        /// The list of dependent modules.
        /// </summary>
        public List<IModule> DependentModules { get; }

        /// <summary>
        /// The list of assemblies that contain AutoMapper profiles.
        /// </summary>
        public List<Assembly> AutomapperAssemblies { get; }

        /// <summary>
        /// The list of assemblies that contain views.
        /// </summary>
        public List<Assembly> ViewAssemblies { get; }
    }
}