using System.Reflection;

namespace ServiceBricks.Security
{
    /// <summary>
    /// The module definition for the Security API Brick.
    /// </summary>
    public partial class SecurityModule : IModule
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public SecurityModule()
        {
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