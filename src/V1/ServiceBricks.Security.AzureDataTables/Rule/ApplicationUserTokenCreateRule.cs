using Microsoft.Extensions.Logging;
using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is a business rule for the ApplicationUserToken object to set the
    /// partitionkey and rowkey of the object before create.
    /// </summary>
    public partial class ApplicationUserTokenCreateRule : BusinessRule
    {
        /// <summary>
        /// Internal.
        /// </summary>
        protected readonly ILogger _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public ApplicationUserTokenCreateRule(
            ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ApplicationUserTokenCreateRule>();
            Priority = PRIORITY_LOW;
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void RegisterRule(IBusinessRuleRegistry registry)
        {
            registry.RegisterItem(
                typeof(DomainCreateBeforeEvent<ApplicationUserToken>),
                typeof(ApplicationUserTokenCreateRule));
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
                if (context.Object is DomainCreateBeforeEvent<ApplicationUserToken> ei)
                {
                    var item = ei.DomainObject;
                    item.PartitionKey = item.UserId.ToString();
                    item.RowKey =
                        item.LoginProvider +
                        StorageAzureDataTablesConstants.KEY_DELIMITER +
                        item.Name;
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