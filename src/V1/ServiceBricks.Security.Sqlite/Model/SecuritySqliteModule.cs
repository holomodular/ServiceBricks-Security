using ServiceBricks.Security.EntityFrameworkCore;
using System.Reflection;

namespace ServiceBricks.Security.Sqlite
{
    /// <summary>
    /// THe module definition for the ServiceBricks Security Sqlite module.
    /// </summary>
    public partial class SecuritySqliteModule : ServiceBricks.Module
    {
        /// <summary>
        /// Instance.
        /// </summary>
        public static SecuritySqliteModule Instance = new SecuritySqliteModule();

        /// <summary>
        /// Constructor.
        /// </summary>
        public SecuritySqliteModule()
        {
            DependentModules = new List<IModule>()
            {
                new SecurityEntityFrameworkCoreModule()
            };
        }
    }
}