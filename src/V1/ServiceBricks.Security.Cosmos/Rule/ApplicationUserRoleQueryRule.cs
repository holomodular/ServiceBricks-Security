using Microsoft.Extensions.Logging;
using ServiceBricks.Storage.EntityFrameworkCore;
using ServiceQuery;

namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// This is a business rule for the ApplicationUserRole domain object when querying.
    /// </summary>
    public sealed class ApplicationUserRoleQueryRule : BusinessRule
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public ApplicationUserRoleQueryRule(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ApplicationUserRoleQueryRule>();
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void RegisterRule(IBusinessRuleRegistry registry)
        {
            registry.RegisterItem(
                typeof(DomainQueryBeforeEvent<ApplicationUserRole>),
                typeof(ApplicationUserRoleQueryRule));
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
                if (context.Object is DomainQueryBeforeEvent<ApplicationUserRole> ei)
                {
                    var item = ei.DomainObject;
                    if (ei.ServiceQueryRequest == null || ei.ServiceQueryRequest.Filters == null)
                        return response;

                    for (int i = 0; i < ei.ServiceQueryRequest.Filters.Count; i++)
                    {
                        if (ei.ServiceQueryRequest.Filters[i].Properties != null &&
                        ei.ServiceQueryRequest.Filters[i].Properties.Count > 0)
                        {
                            bool found = false;
                            foreach (var prop in ei.ServiceQueryRequest.Filters[i].Properties)
                            {
                                if (string.Compare(prop, "StorageKey", true) == 0)
                                    found = true;
                            }

                            if (found)
                            {
                                if (ei.ServiceQueryRequest.Filters[i].Values != null &&
                                    ei.ServiceQueryRequest.Filters[i].Values.Count >= 1)
                                {
                                    var q = BuildKeyQuery(ei.ServiceQueryRequest.Filters[i].Values[0]);
                                    if (q != null)
                                    {
                                        ei.ServiceQueryRequest.Filters.RemoveAt(i);
                                        ei.ServiceQueryRequest.Filters.InsertRange(i, q.Filters);
                                        i += q.Filters.Count;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_BUSINESS_RULE));
            }

            return response;
        }

        private ServiceQueryRequest BuildKeyQuery(string storageKey)
        {
            string[] split = storageKey.Split(StorageEntityFrameworkCoreConstants.STORAGEKEY_DELIMITER);
            if (split.Length != 2)
                return null;

            string userKey = split[0];
            string roleKey = split[1];

            var qb = ServiceQueryRequestBuilder.New()
                .BeginExpression()
                .IsEqual(nameof(ApplicationUserRole.UserId), userKey)
                .And()
                .IsEqual(nameof(ApplicationUserRole.RoleId), roleKey)
                .EndExpression();

            return qb.Build();
        }
    }
}