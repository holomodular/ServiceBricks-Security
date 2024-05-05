using Microsoft.Extensions.Logging;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is a business rule for the ApplicationRole object to set the
    /// partitionkey and rowkey of the object before create.
    /// </summary>
    public partial class ApplicationRoleCreateRule : BusinessRule
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
        public ApplicationRoleCreateRule(ILoggerFactory loggerFactory, ITimezoneService timezoneService)
        {
            _logger = loggerFactory.CreateLogger<ApplicationRoleCreateRule>();
            _timezoneService = timezoneService;
            Priority = PRIORITY_LOW;
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void RegisterRule(IBusinessRuleRegistry registry)
        {
            registry.RegisterItem(
                typeof(DomainCreateBeforeEvent<ApplicationRole>),
                typeof(ApplicationRoleCreateRule));
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
                if (context.Object is DomainCreateBeforeEvent<ApplicationRole> ei)
                {
                    var item = ei.DomainObject;
                    item.Id = Guid.NewGuid();
                    item.PartitionKey = item.Id.ToString();
                    item.RowKey = "";
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