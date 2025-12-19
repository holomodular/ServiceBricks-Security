using ServiceBricks;

//using ServiceBricks.Logging;
using ServiceBricks.Security;

namespace WebApp.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        private static IApplicationBuilder RegisterMiddleware(this IApplicationBuilder app)
        {
            //app.UseMiddleware<CustomLoggerMiddleware>();
            //app.UseMiddleware<WebRequestMessageMiddleware>();
            //app.UseMiddleware<PropogateExceptionResponseMiddleware>();
            app.UseMiddleware<TrapExceptionResponseMiddleware>();
            return app;
        }

        public static IApplicationBuilder StartCustomWebsite(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(x =>
                {
                    x.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                    x.SwaggerEndpoint("/swagger/v2/swagger.json", "API v2");
                });
            }

            if (!env.IsDevelopment())
                app.UseHsts();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            // Register Middleware after UseAuth() so user context is available
            app.RegisterMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });

            // Create a default test user account
            app.CreateTestUserAccount();

            return app;
        }

        private static IApplicationBuilder CreateTestUserAccount(this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<IUserManagerService>();
                var respFind = userManager.FindByEmail("unittest@servicebricks.com");
                if (respFind.Item == null)
                {
                    var testUser = new UserDto()
                    {
                        Email = "unittest@servicebricks.com",
                        UserName = "unittest@servicebricks.com",
                        PhoneNumber = "1234567890",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                    };
                    var respCreate = userManager.Create(testUser, "UnitTest123!@#");
                    if (respCreate.Success && respCreate.Item != null)
                        userManager.AddToRole(respCreate.Item.StorageKey, ServiceBricksConstants.SECURITY_ROLE_ADMIN_NAME);
                }
            }

            return builder;
        }
    }
}