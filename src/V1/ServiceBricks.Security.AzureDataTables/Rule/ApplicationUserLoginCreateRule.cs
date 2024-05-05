using Microsoft.Extensions.Logging;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is a business rule for the ApplicationUserLogin object to set the
    /// partitionkey and rowkey of the object before create.
    /// </summary>
    public partial class ApplicationUserLoginCreateRule : BusinessRule
    {
        /// <summary>
        /// Internal.
        /// </summary>
        protected readonly ILogger _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public ApplicationUserLoginCreateRule(
            ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ApplicationUserLoginCreateRule>();
            Priority = PRIORITY_LOW;
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void RegisterRule(IBusinessRuleRegistry registry)
        {
            registry.RegisterItem(
                typeof(DomainCreateBeforeEvent<ApplicationUserLogin>),
                typeof(ApplicationUserLoginCreateRule));
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
                if (context.Object is DomainCreateBeforeEvent<ApplicationUserLogin> ei)
                {
                    var item = ei.DomainObject;
                    item.PartitionKey = item.LoginProvider;
                    item.RowKey = item.ProviderKey;
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