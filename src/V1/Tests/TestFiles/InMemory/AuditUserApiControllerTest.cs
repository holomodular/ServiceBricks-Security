using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;

namespace ServiceBricks.Xunit
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class AuditUserApiControllerTest : ApiControllerTest<AuditUserDto>
    {
        public AuditUserApiControllerTest() : base()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupInMemory));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<AuditUserDto>>();
            CreateDependencies();
        }

        protected virtual void CreateDependencies()
        {
            var appUserTest = new ApplicationUserApiControllerTest();
            appUserTest.SystemManager = this.SystemManager;
            var user = appUserTest.TestManager.GetMinimumDataObject();
            ((AuditUserTestManager)TestManager).ApplicationUser =
                appUserTest.CreateBase(user);
        }

        [Fact]
        public virtual async Task Update_CreateDate()
        {
            var model = TestManager.GetMinimumDataObject();
            var dto = await CreateBaseAsync(model);

            DateTimeOffset startingCreateDate = dto.CreateDate;

            //Update the CreateDate property
            dto.CreateDate = DateTimeOffset.UtcNow;

            //Call Update
            var controller = TestManager.GetController(SystemManager.ServiceProvider);
            var respUpdate = await controller.UpdateAsync(dto);
            if (respUpdate is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is AuditUserDto obj)
                {
                    Assert.True(obj.CreateDate == startingCreateDate);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }
    }
}