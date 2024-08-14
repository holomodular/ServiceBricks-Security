using Microsoft.Extensions.Logging;
using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.Security.AzureDataTables
{
    public sealed class ApplicationUserClaimQueryRule : BusinessRule
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public ApplicationUserClaimQueryRule(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ApplicationUserClaimQueryRule>();
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void RegisterRule(IBusinessRuleRegistry registry)
        {
            registry.RegisterItem(
                typeof(DomainQueryBeforeEvent<ApplicationUserClaim>),
                typeof(ApplicationUserClaimQueryRule));
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
                if (context.Object is DomainQueryBeforeEvent<ApplicationUserClaim> ei)
                {
                    var item = ei.DomainObject;
                    if (ei.ServiceQueryRequest == null || ei.ServiceQueryRequest.Filters == null)
                        return response;
                    foreach (var filter in ei.ServiceQueryRequest.Filters)
                    {
                        bool found = false;
                        if (filter.Properties != null &&
                            filter.Properties.Count > 0)
                        {
                            for (int i = 0; i < filter.Properties.Count; i++)
                            {
                                if (string.Compare(filter.Properties[i], "StorageKey", true) == 0)
                                {
                                    found = true;
                                    filter.Properties[i] = "RowKey";
                                }
                            }
                            if (found)
                            {
                                if (filter.Values != null && filter.Values.Count > 0)
                                {
                                    for (int i = 0; i < filter.Values.Count; i++)
                                    {
                                        string[] split = filter.Values[i].Split(StorageAzureDataTablesConstants.STORAGEKEY_DELIMITER);
                                        if (split.Length == 2)
                                        {
                                            filter.Values[i] = split[1];
                                        }
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
    }
}