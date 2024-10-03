using System.Reflection;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// The module definition for the Security Entity Framework Core module.
    /// </summary>
    public partial class SecurityEntityFrameworkCoreModule : ServiceBricks.Module
    {
        /// <summary>
        /// Instance
        /// </summary>
        public static SecurityEntityFrameworkCoreModule Instance = new SecurityEntityFrameworkCoreModule();

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
    }
}