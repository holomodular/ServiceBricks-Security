using System.Reflection;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// The module definition for the ServiceBricks.Security.AzureDataTables namespace.
    /// </summary>
    public partial class SecurityAzureDataTablesModule : IModule
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public SecurityAzureDataTablesModule()
        {
            AutomapperAssemblies = new List<Assembly>()
            {
                typeof(SecurityAzureDataTablesModule).Assembly
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
        /// The list of automapper assemblies.
        /// </summary>
        public List<Assembly> AutomapperAssemblies { get; }

        /// <summary>
        /// The list of view assemblies.
        /// </summary>
        public List<Assembly> ViewAssemblies { get; }
    }
}