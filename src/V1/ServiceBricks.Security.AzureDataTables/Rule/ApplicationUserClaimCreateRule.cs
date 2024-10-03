namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is a business rule for the ApplicationUserClaim object to set the
    /// partitionkey and rowkey of the object before create.
    /// </summary>
    public sealed class ApplicationUserClaimCreateRule : BusinessRule
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ApplicationUserClaimCreateRule()
        {
            Priority = PRIORITY_LOW;
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(DomainCreateBeforeEvent<ApplicationUserClaim>),
                typeof(ApplicationUserClaimCreateRule));
        }

        /// <summary>
        /// Unregister the rule
        /// </summary>
        /// <param name="registry"></param>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(DomainCreateBeforeEvent<ApplicationUserClaim>),
                typeof(ApplicationUserClaimCreateRule));
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
            var ei = context.Object as DomainCreateBeforeEvent<ApplicationUserClaim>;
            if (ei == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            var item = ei.DomainObject;
            item.Key = Guid.NewGuid();
            item.PartitionKey = item.UserId.ToString();
            item.RowKey = item.Key.ToString();

            return response;
        }
    }
}