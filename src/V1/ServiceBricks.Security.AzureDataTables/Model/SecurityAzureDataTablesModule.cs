using System.Reflection;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// The module definition for the ServiceBricks.Security.AzureDataTables namespace.
    /// </summary>
    public partial class SecurityAzureDataTablesModule : ServiceBricks.Module
    {
        /// <summary>
        /// Instance.
        /// </summary>
        public static SecurityAzureDataTablesModule Instance = new SecurityAzureDataTablesModule();

        /// <summary>
        /// Constructor.
        /// </summary>
        public SecurityAzureDataTablesModule()
        {
            DependentModules = new List<IModule>()
            {
                new SecurityModule()
            };
        }
    }
}