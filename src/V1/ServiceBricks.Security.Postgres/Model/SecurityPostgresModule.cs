using ServiceBricks.Security.EntityFrameworkCore;
using System.Reflection;

namespace ServiceBricks.Security.Postgres
{
    /// <summary>
    /// The module definition for the ServiceBricks Security Postgres module.
    /// </summary>
    public partial class SecurityPostgresModule : IModule
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public SecurityPostgresModule()
        {
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
        /// The list of automapper assemblies.
        /// </summary>
        public List<Assembly> AutomapperAssemblies { get; }

        /// <summary>
        /// The list of view assemblies.
        /// </summary>
        public List<Assembly> ViewAssemblies { get; }
    }
}