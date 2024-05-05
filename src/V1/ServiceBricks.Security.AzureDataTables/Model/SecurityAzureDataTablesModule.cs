using System.Reflection;

namespace ServiceBricks.Security.AzureDataTables
{
    public class SecurityAzureDataTablesModule : IModule
    {
        public SecurityAzureDataTablesModule()
        {
            AdminHtml = string.Empty;
            Name = "Security AzureDataTables Brick";
            Description = @"The Security AzureDataTables Brick implements the AzureDataTables provider.";
            AutomapperAssemblies = new List<Assembly>()
            {
                typeof(SecurityAzureDataTablesModule).Assembly
            };
            DependentModules = new List<IModule>()
            {
                new SecurityModule()
            };
        }

        public string Name { get; }
        public string Description { get; }
        public string AdminHtml { get; }
        public List<Assembly> AutomapperAssemblies { get; }
        public List<Assembly> ViewAssemblies { get; }
        public List<IModule> DependentModules { get; }
    }
}