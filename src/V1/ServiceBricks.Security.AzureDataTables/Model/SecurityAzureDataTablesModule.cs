using System.Reflection;

namespace ServiceBricks.Security.AzureDataTables
{
    public class SecurityAzureDataTablesModule : IModule
    {
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

        public List<Assembly> AutomapperAssemblies { get; }
        public List<Assembly> ViewAssemblies { get; }
        public List<IModule> DependentModules { get; }
    }
}