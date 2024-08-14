using System.Reflection;

namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// The module definition for the Security Cosmos module.
    /// </summary>
    public partial class SecurityCosmosModule : IModule
    {
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

        /// <summary>
        /// The list of dependent modules.
        /// </summary>
        public List<IModule> DependentModules { get; }

        /// <summary>
        /// The list of Automapper assemblies.
        /// </summary>
        public List<Assembly> AutomapperAssemblies { get; }

        /// <summary>
        /// The list of view assemblies.
        /// </summary>
        public List<Assembly> ViewAssemblies { get; }
    }
}