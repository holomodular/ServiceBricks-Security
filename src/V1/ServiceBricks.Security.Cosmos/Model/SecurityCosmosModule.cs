using System.Reflection;

namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// The module definition for the Security Cosmos module.
    /// </summary>
    public partial class SecurityCosmosModule : ServiceBricks.Module
    {
        /// <summary>
        /// Instance.
        /// </summary>
        public static SecurityCosmosModule Instance = new SecurityCosmosModule();

        /// <summary>
        /// Constructor.
        /// </summary>
        public SecurityCosmosModule()
        {
            AutomapperAssemblies = new List<Assembly>()
            {
                typeof(SecurityCosmosModule).Assembly
            };
            DependentModules = new List<IModule>()
            {
                new SecurityModule()
            };
        }
    }
}