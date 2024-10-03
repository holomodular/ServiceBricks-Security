using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is a business rule for the AuditUser object to set the partitionkey and rowkey of the object before create.
    /// </summary>
    public sealed class UserAuditCreateRule : BusinessRule
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public UserAuditCreateRule()
        {
            Priority = PRIORITY_LOW;
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(DomainCreateBeforeEvent<UserAudit>),
                typeof(UserAuditCreateRule));
        }

        /// <summary>
        /// Unregister the rule
        /// </summary>
        /// <param name="registry"></param>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(DomainCreateBeforeEvent<UserAudit>),
                typeof(UserAuditCreateRule));
        }

        /// <summary>
        /// Execute the business rule.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override IResponse ExecuteRule(IBusinessRuleContext context)
        {
            var response = new Response();

            // AI: Make sure the context object is the correct type
            if (context == null || context.Object == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }
            var ei = context.Object as DomainCreateBeforeEvent<UserAudit>;
            if (ei == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            var item = ei.DomainObject;
            item.Key = Guid.NewGuid();
            item.PartitionKey = item.UserId.ToString();
            var reverseDate = DateTimeOffset.MaxValue.Ticks - item.CreateDate.Ticks;
            item.RowKey =
                reverseDate.ToString("d19") +
                StorageAzureDataTablesConstants.KEY_DELIMITER +
                item.Key.ToString();

            return response;
        }
    }
}