using ServiceBricks.Security.EntityFrameworkCore;
using System.Reflection;

namespace ServiceBricks.Security.Sqlite
{
    /// <summary>
    /// THe module definition for the ServiceBricks Security Sqlite module.
    /// </summary>
    public partial class SecuritySqliteModule : IModule
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public SecuritySqliteModule()
        {
            AutomapperAssemblies = new List<Assembly>()
            {
                typeof(SecuritySqliteModule).Assembly
            };
            DependentModules = new List<IModule>()
            {
                new SecurityEntityFrameworkCoreModule()
            };
        }

        /// <summary>
        /// THe list of dependent modules.
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