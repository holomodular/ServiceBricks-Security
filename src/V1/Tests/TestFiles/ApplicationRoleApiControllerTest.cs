using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public abstract class ApplicationRoleApiControllerTestBase : ApiControllerTest<ApplicationRoleDto>
    {
        public ApplicationRoleApiControllerTestBase() : base()
        {
        }
    }
}