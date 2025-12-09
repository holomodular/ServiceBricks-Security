using System.Reflection;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// The module definition for the ServiceBricks Security MongoDb module.
    /// </summary>
    public partial class SecurityMongoDbModule : ServiceBricks.Module
    {
        /// <summary>
        /// Instance.
        /// </summary>
        public static SecurityMongoDbModule Instance = new SecurityMongoDbModule();

        /// <summary>
        /// Constructor.
        /// </summary>
        public SecurityMongoDbModule()
        {
            DependentModules = new List<IModule>()
            {
                new SecurityModule()
            };
        }
    }
}