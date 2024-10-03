using ServiceBricks.Security.EntityFrameworkCore;

namespace ServiceBricks.Security.Postgres
{
    /// <summary>
    /// The module definition for the ServiceBricks Security Postgres module.
    /// </summary>
    public partial class SecurityPostgresModule : ServiceBricks.Module
    {
        /// <summary>
        /// Instance.
        /// </summary>
        public static SecurityPostgresModule Instance = new SecurityPostgresModule();

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
    }
}