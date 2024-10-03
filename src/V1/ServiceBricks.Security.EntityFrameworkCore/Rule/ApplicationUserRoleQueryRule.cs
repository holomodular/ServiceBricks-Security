using ServiceBricks.Storage.EntityFrameworkCore;
using ServiceQuery;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// This is a business rule that is executed before a query is executed for an ApplicationUserRole.
    /// </summary>
    public sealed class ApplicationUserRoleQueryRule : BusinessRule
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ApplicationUserRoleQueryRule()
        {
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(DomainQueryBeforeEvent<ApplicationUserRole>),
                typeof(ApplicationUserRoleQueryRule));
        }

        /// <summary>
        /// Unregister a rule
        /// </summary>
        /// <param name="registry"></param>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(DomainQueryBeforeEvent<ApplicationUserRole>),
                typeof(ApplicationUserRoleQueryRule));
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
            var ei = context.Object as DomainQueryBeforeEvent<ApplicationUserRole>;
            if (ei == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

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

            return response;
        }

        private ServiceQueryRequest BuildKeyQuery(string storageKey)
        {
            string[] split = storageKey.Split(StorageEntityFrameworkCoreConstants.STORAGEKEY_DELIMITER);
            if (split.Length != 2)
                return null;

            string userKey = split[0];
            string roleKey = split[1];

            var qb = ServiceQueryRequestBuilder.New()
                .Begin()
                    .IsEqual(nameof(ApplicationUserRole.UserId), userKey)
                    .And()
                    .IsEqual(nameof(ApplicationUserRole.RoleId), roleKey)
                .End();

            return qb.Build();
        }
    }
}