using ServiceBricks.Security.EntityFrameworkCore;

namespace ServiceBricks.Security.SqlServer
{
    /// <summary>
    /// Module
    /// </summary>
    public partial class SecuritySqlServerModule : ServiceBricks.Module
    {
        /// <summary>
        /// Instance.
        /// </summary>
        public static SecuritySqlServerModule Instance = new SecuritySqlServerModule();

        /// <summary>
        /// Constructor.
        /// </summary>
        public SecuritySqlServerModule()
        {
            DependentModules = new List<IModule>()
            {
                new SecurityEntityFrameworkCoreModule()
            };
        }
    }
}