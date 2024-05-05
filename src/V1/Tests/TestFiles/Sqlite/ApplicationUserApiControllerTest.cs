using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class ApplicationUserApiControllerTest : ApiControllerTest<ApplicationUserDto>
    {
        public ApplicationUserApiControllerTest() : base()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupSqlite));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<ApplicationUserDto>>();
        }

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
                if (okResult.Value is ApplicationUserDto obj)
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