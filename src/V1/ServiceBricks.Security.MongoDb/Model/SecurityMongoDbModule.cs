using System.Reflection;

namespace ServiceBricks.Security.MongoDb
{
    public class SecurityMongoDbModule : IModule
    {
        public SecurityMongoDbModule()
        {
            AdminHtml = string.Empty;
            Name = "Security MongoDB Brick";
            Description = @"The Security MongoDB Brick implements the MongoDB provider.";
            AutomapperAssemblies = new List<Assembly>()
            {
                typeof(SecurityMongoDbModule).Assembly
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