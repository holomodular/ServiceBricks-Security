using Microsoft.Extensions.Logging;
using ServiceBricks.Storage.AzureDataTables;
using ServiceQuery;

namespace ServiceBricks.Security.AzureDataTables
{
    public sealed class ApplicationUserTokenQueryRule : BusinessRule
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public ApplicationUserTokenQueryRule(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ApplicationUserTokenQueryRule>();
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void RegisterRule(IBusinessRuleRegistry registry)
        {
            registry.RegisterItem(
                typeof(DomainQueryBeforeEvent<ApplicationUserToken>),
                typeof(ApplicationUserTokenQueryRule));
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
                if (context.Object is DomainQueryBeforeEvent<ApplicationUserToken> ei)
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
            string[] split = storageKey.Split(StorageAzureDataTablesConstants.STORAGEKEY_DELIMITER);
            if (split.Length != 2)
                return null;

            string[] split2 = split[1].Split(StorageAzureDataTablesConstants.KEY_DELIMITER);
            if (split2.Length != 2)
                return null;
            string userkey = split[0];
            string loginProvider = split2[0];
            string name = split2[1];

            var qb = ServiceQueryRequestBuilder.New()
                .BeginExpression()
                .IsEqual(nameof(ApplicationUserToken.UserId), userkey)
                .And()
                .IsEqual(nameof(ApplicationUserToken.LoginProvider), loginProvider)
                .And()
                .IsEqual(nameof(ApplicationUserToken.Name), name)
                .EndExpression();

            return qb.Build();
        }
    }
}