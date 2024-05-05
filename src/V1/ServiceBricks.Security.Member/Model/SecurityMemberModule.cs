using System.Collections.Generic;
using System.Reflection;

namespace ServiceBricks.Security.Member
{
    public class SecurityMemberModule : IModule
    {
        public SecurityMemberModule()
        {
            AdminHtml = string.Empty;
            Name = "Security Member Brick";
            Description = @"The Security Member Brick is responsible for application security using JSON Web Tokens (JWT).";
        }

        public string Name { get; }
        public string Description { get; }
        public string AdminHtml { get; }
        public List<Assembly> AutomapperAssemblies { get; }
        public List<Assembly> ViewAssemblies { get; }
        public List<IModule> DependentModules { get; }
    }
}