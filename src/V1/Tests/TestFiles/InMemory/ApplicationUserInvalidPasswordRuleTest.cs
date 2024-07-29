using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Xunit;
using ServiceQuery;
using ServiceBricks.Security;
using ServiceBricks.Xunit.Integration;

namespace ServiceBricks.Xunit.Rules
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public partial class ApplicationUserInvalidPasswordRuleTest : ApplicationUserInvalidPasswordRuleTestBase
    {
        public ApplicationUserInvalidPasswordRuleTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupInMemory));
        }
    }
}