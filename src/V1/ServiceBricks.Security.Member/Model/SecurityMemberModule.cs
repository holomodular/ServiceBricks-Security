using System.Reflection;

namespace ServiceBricks.Security.Member
{
    /// <summary>
    /// The module definition for the ServiceBricks Security Member module.
    /// </summary>
    public class SecurityMemberModule : IModule
    {
        /// <summary>
        /// The list of dependent modules.
        /// </summary>
        public List<IModule> DependentModules { get; }

        /// <summary>
        /// The list of assemblies that contain AutoMapper profiles.
        /// </summary>
        public List<Assembly> AutomapperAssemblies { get; }

        /// <summary>
        /// The list of assemblies that contain views.
        /// </summary>
        public List<Assembly> ViewAssemblies { get; }
    }
}