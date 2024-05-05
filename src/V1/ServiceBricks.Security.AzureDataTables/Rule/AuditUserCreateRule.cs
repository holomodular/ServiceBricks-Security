using Microsoft.Extensions.Logging;
using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is a business rule for the AuditUser object to set the
    /// partitionkey and rowkey of the object before create.
    /// </summary>
    public partial class AuditUserCreateRule : BusinessRule
    {
        /// <summary>
        /// Internal.
        /// </summary>
        protected readonly ILogger _logger;

        private readonly ITimezoneService _timezoneService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public AuditUserCreateRule(ILoggerFactory loggerFactory, ITimezoneService timezoneService)
        {
            _logger = loggerFactory.CreateLogger<AuditUserCreateRule>();
            _timezoneService = timezoneService;
            Priority = PRIORITY_LOW;
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void RegisterRule(IBusinessRuleRegistry registry)
        {
            registry.RegisterItem(
                typeof(DomainCreateBeforeEvent<AuditUser>),
                typeof(AuditUserCreateRule));
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
                if (context.Object is DomainCreateBeforeEvent<AuditUser> ei)
                {
                    var item = ei.DomainObject;
                    item.Key = Guid.NewGuid();
                    item.PartitionKey = item.UserId.ToString();
                    var reverseDate = DateTimeOffset.MaxValue.Ticks - item.CreateDate.Ticks;
                    item.RowKey =
                        reverseDate.ToString("d19") +
                        StorageAzureDataTablesConstants.KEY_DELIMITER +
                        item.Key.ToString();
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