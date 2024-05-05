using Microsoft.Extensions.Logging;
using ServiceQuery;
using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.Security.AzureDataTables
{
    public partial class ApplicationUserLoginQueryRule : BusinessRule
    {
        /// <summary>
        /// Internal.
        /// </summary>
        protected readonly ILogger _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public ApplicationUserLoginQueryRule(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ApplicationUserLoginQueryRule>();
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void RegisterRule(IBusinessRuleRegistry registry)
        {
            registry.RegisterItem(
                typeof(DomainQueryBeforeEvent<ApplicationUserLogin>),
                typeof(ApplicationUserLoginQueryRule));
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
                if (context.Object is DomainQueryBeforeEvent<ApplicationUserLogin> ei)
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
                            for (var j = 0; j < ei.ServiceQueryRequest.Filters[i].Properties.Count; j++)
                            {
                                if (string.Compare(ei.ServiceQueryRequest.Filters[i].Properties[j], "UserStorageKey", true) == 0)
                                    ei.ServiceQueryRequest.Filters[i].Properties[j] = "UserId";
                            }
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

                    foreach (var filter in ei.ServiceQueryRequest.Filters)
                    {
                        if (filter.Properties != null &&
                            filter.Properties.Count > 0)
                        {
                            for (int i = 0; i < filter.Properties.Count; i++)
                            {
                                if (string.Compare(filter.Properties[i], "StorageKey", true) == 0)
                                    filter.Properties[i] = "Id";
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

            string loginProvider = split[0];
            string provider = split[1];

            var qb = ServiceQueryRequestBuilder.New()
                .BeginExpression()
                .IsEqual(nameof(ApplicationUserLogin.LoginProvider), loginProvider)
                .And()
                .IsEqual(nameof(ApplicationUserLogin.ProviderKey), provider)
                .EndExpression();

            return qb.Build();
        }
    }
}