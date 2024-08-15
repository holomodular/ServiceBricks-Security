using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This business rule happens when an invalid password event is raised.
    /// </summary>
    public sealed class UserInvalidPasswordRule : BusinessRule
    {
        private readonly ILogger _logger;
        private readonly IUserAuditApiService _auditUserApiService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIpAddressService _iPAddressService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public UserInvalidPasswordRule(
            ILoggerFactory loggerFactory,
            IUserAuditApiService auditUserApiService,
            IHttpContextAccessor httpContextAccessor,
            IIpAddressService iPAddressService
            )
        {
            _logger = loggerFactory.CreateLogger<UserInvalidPasswordRule>();
            _auditUserApiService = auditUserApiService;
            _httpContextAccessor = httpContextAccessor;
            _iPAddressService = iPAddressService;
            Priority = PRIORITY_LOW;
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void RegisterRule(IBusinessRuleRegistry registry)
        {
            registry.RegisterItem(
                typeof(UserInvalidPasswordProcess),
                typeof(UserInvalidPasswordRule));
        }

        /// <summary>
        /// Execute the business rule.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override IResponse ExecuteRule(IBusinessRuleContext context)
        {
            var response = new Response();

            try
            {
                // AI: Make sure the context object is the correct type
                var uipp = context.Object as UserInvalidPasswordProcess;
                if (uipp != null)
                {
                    // AI: Determine if user was found
                    if (!string.IsNullOrEmpty(uipp.UserStorageKey))
                    {
                        // AI: Audit found user
                        _auditUserApiService.Create(new UserAuditDto()
                        {
                            AuditType = AuditType.INVALID_PASSWORD_TEXT,
                            RequestHeaders = _httpContextAccessor?.HttpContext?.Request?.Headers?.GetData(),
                            UserStorageKey = uipp.UserStorageKey,
                            IPAddress = _iPAddressService.GetIPAddress(),
                        });
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(uipp.Email))
                        {
                            // AI: Audit not found user
                            _auditUserApiService.Create(new UserAuditDto()
                            {
                                AuditType = AuditType.INVALID_PASSWORD_TEXT,
                                RequestHeaders = _httpContextAccessor?.HttpContext?.Request?.Headers?.GetData(),
                                IPAddress = _iPAddressService.GetIPAddress(),
                                Data = uipp.Email
                            });
                        }
                    }
                    return response;
                }
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