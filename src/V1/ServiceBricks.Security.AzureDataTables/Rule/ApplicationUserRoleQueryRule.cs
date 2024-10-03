﻿using ServiceBricks.Storage.AzureDataTables;
using ServiceQuery;

namespace ServiceBricks.Security.AzureDataTables
{
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
        /// Unregister the rule
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
                    for (var j = 0; j < ei.ServiceQueryRequest.Filters[i].Properties.Count; j++)
                    {
                        if (string.Compare(ei.ServiceQueryRequest.Filters[i].Properties[j], "UserStorageKey", true) == 0)
                            ei.ServiceQueryRequest.Filters[i].Properties[j] = "PartitionKey";
                        if (string.Compare(ei.ServiceQueryRequest.Filters[i].Properties[j], "RoleStorageKey", true) == 0)
                            ei.ServiceQueryRequest.Filters[i].Properties[j] = "RowKey";
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

            return response;
        }

        private ServiceQueryRequest BuildKeyQuery(string storageKey)
        {
            string[] split = storageKey.Split(StorageAzureDataTablesConstants.STORAGEKEY_DELIMITER);
            if (split.Length != 2)
                return null;

            string userKey = split[0];
            string roleKey = split[1];

            var qb = ServiceQueryRequestBuilder.New()
                .BeginExpression()
                .IsEqual(nameof(ApplicationUserRole.UserId), userKey)
                .And()
                .IsEqual(nameof(ApplicationUserRole.RoleId), roleKey)
                .EndExpression();

            return qb.Build();
        }
    }
}