using System.Reflection;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// The module definition for the Security Entity Framework Core module.
    /// </summary>
    public partial class SecurityEntityFrameworkCoreModule : IModule
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public SecurityEntityFrameworkCoreModule()
        {
            AutomapperAssemblies = new List<Assembly>()
            {
                typeof(SecurityEntityFrameworkCoreModule).Assembly
            };
            DependentModules = new List<IModule>()
            {
                new SecurityModule()
            };
        }

        /// <summary>
        /// The list of dependent modules.
        /// </summary>
        public List<IModule> DependentModules { get; }

        /// <summary>
        /// The list of assemblies that contain Automapper profiles.
        /// </summary>
        public List<Assembly> AutomapperAssemblies { get; }

        /// <summary>
        /// The list of assemblies that contain views.
        /// </summary>
        public List<Assembly> ViewAssemblies { get; }
    }
}