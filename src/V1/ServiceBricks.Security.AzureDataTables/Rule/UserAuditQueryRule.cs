using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.Security.AzureDataTables
{
    public sealed class UserAuditQueryRule : BusinessRule
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public UserAuditQueryRule()
        {
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(DomainQueryBeforeEvent<UserAudit>),
                typeof(UserAuditQueryRule));
        }

        /// <summary>
        /// Unregister the rule
        /// </summary>
        /// <param name="registry"></param>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(DomainQueryBeforeEvent<UserAudit>),
                typeof(UserAuditQueryRule));
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
            var ei = context.Object as DomainQueryBeforeEvent<UserAudit>;
            if (ei == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

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
                            filter.Properties[i] = "Key";
                        }
                    }
                }
                if (found)
                {
                    if (filter.Values != null && filter.Values.Count > 0)
                    {
                        for (int i = 0; i < filter.Values.Count; i++)
                        {
                            string[] split = filter.Values[i].Split(StorageAzureDataTablesConstants.KEY_DELIMITER);
                            if (split.Length == 2)
                            {
                                filter.Values[i] = split[1];
                            }
                        }
                    }
                }
            }

            return response;
        }
    }
}