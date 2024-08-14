using System.Reflection;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// The module definition for the ServiceBricks Security MongoDb module.
    /// </summary>
    public partial class SecurityMongoDbModule : IModule
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public SecurityMongoDbModule()
        {
            AutomapperAssemblies = new List<Assembly>()
            {
                typeof(SecurityMongoDbModule).Assembly
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
        /// The list of assemblies that contain AutoMapper profiles.
        /// </summary>
        public List<Assembly> AutomapperAssemblies { get; }

        /// <summary>
        /// The list of assemblies that contain views.
        /// </summary>
        public List<Assembly> ViewAssemblies { get; }
    }
}