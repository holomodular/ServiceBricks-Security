using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using System;
using System.Threading.Tasks;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This business rule happens when an invalid password event is raised.
    /// </summary>
    public partial class UserInvalidPasswordRule : BusinessRule
    {
        /// <summary>
        /// Internal.
        /// </summary>
        protected readonly ILogger _logger;

        private readonly IAuditUserApiService _auditUserApiService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIpAddressService _iPAddressService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public UserInvalidPasswordRule(
            ILoggerFactory loggerFactory,
            IAuditUserApiService auditUserApiService,
            IHttpContextAccessor httpContextAccessor,
            IIpAddressService iPAddressService
            )
        {
            _logger = loggerFactory.CreateLogger<UserInvalidPasswordRule>();
            _auditUserApiService = auditUserApiService;
            _httpContextAccessor = httpContextAccessor;
            _iPAddressService = iPAddressService;
            Priority = PRIORITY_NORMAL;
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
                var e = context.Object as UserInvalidPasswordProcess;
                if (e == null)
                    return response;

                if (!string.IsNullOrEmpty(e.UserStorageKey))
                {
                    // Audit
                    _auditUserApiService.Create(new AuditUserDto()
                    {
                        AuditName = AuditType.INVALID_PASSWORD,
                        UserAgent = _httpContextAccessor?.HttpContext?.Request?.Headers?.UserAgent,
                        UserStorageKey = e.UserStorageKey,
                        IPAddress = _iPAddressService.GetIPAddress(),
                    });
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