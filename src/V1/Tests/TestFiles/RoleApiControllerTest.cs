using ServiceBricks.Security;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public abstract class RoleApiControllerTestBase : ApiControllerTest<RoleDto>
    {
        public RoleApiControllerTestBase() : base()
        {
        }
    }
}