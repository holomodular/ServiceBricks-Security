using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This business rule happens when a user logs in.
    /// </summary>
    public sealed class UserLoginRule : BusinessRule
    {
        private readonly ILogger _logger;
        private readonly IAuditUserApiService _auditUserApiService;
        private readonly IUserManagerService _userManagerService;
        private readonly IIpAddressService _iPAddressService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="auditUserApiService"></param>
        /// <param name="userManagerApiService"></param>
        /// <param name="iPAddressService"></param>
        /// <param name="httpContextAccessor"></param>
        public UserLoginRule(
            ILoggerFactory loggerFactory,
            IAuditUserApiService auditUserApiService,
            IUserManagerService userManagerApiService,
            IIpAddressService iPAddressService,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _logger = loggerFactory.CreateLogger<UserLoginRule>();
            _auditUserApiService = auditUserApiService;
            _userManagerService = userManagerApiService;
            _iPAddressService = iPAddressService;
            _httpContextAccessor = httpContextAccessor;
            Priority = PRIORITY_NORMAL;
        }

        public static void RegisterRule(IBusinessRuleRegistry registry)
        {
            registry.RegisterItem(
                    typeof(UserLoginProcess),
                    typeof(UserLoginRule));
        }

        public override IResponse ExecuteRule(IBusinessRuleContext context)
        {
            return ExecuteRuleAsync(context).GetAwaiter().GetResult();
        }

        public override async Task<IResponse> ExecuteRuleAsync(IBusinessRuleContext context)
        {
            var response = new Response();

            try
            {
                // AI: Make sure the context object is the correct type
                var p = context.Object as UserLoginProcess;
                if (p == null)
                    return response;

                // AI: Find the user
                var respU = await _userManagerService.FindByEmailAsync(p.Email);
                if (respU.Error || respU.Item == null)
                {
                    response.AddMessage(ResponseMessage.CreateError("Invalid login attempt"));
                    return response;
                }
                var user = respU.Item;
                p.ApplicationSigninResult = new ApplicationSigninResult()
                {
                    User = user
                };

                // AI: Determine if email confirmed
                if (!user.EmailConfirmed)
                {
                    p.ApplicationSigninResult.EmailNotConfirmed = true;
                    response.AddMessage(ResponseMessage.CreateError("Email Not Confirmed"));
                    return response;
                }

                IResponseItem<ApplicationSigninResult> respSignin = null;
                if (_httpContextAccessor == null || _httpContextAccessor.HttpContext == null)
                {
                    // AI: This is not a web request, unit test
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.UNIT_TEST));
                    return response;
                }
                else
                {
                    // AI: attempt to sign in
                    respSignin = await _userManagerService.PasswordSignInAsync(
                        p.Email,
                        p.Password,
                        p.RememberMe);
                    p.ApplicationSigninResult = respSignin.Item;
                }
                if (respSignin.Error)
                {
                    response.CopyFrom(respSignin);
                    return response;
                }

                // AI: Audit user
                await _auditUserApiService.CreateAsync(new AuditUserDto()
                {
                    AuditName = AuditType.LOGIN_TEXT,
                    UserAgent = _httpContextAccessor?.HttpContext?.Request?.Headers?.UserAgent,
                    UserStorageKey = respU.Item.StorageKey,
                    IPAddress = _iPAddressService.GetIPAddress()
                });
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_BUSINESS_RULE));
            }
            return response;
        }
    }
}