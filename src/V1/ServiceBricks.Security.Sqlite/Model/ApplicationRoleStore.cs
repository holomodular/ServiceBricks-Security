using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using ServiceQuery;
using System.Security.Claims;
using ServiceBricks.Security.EntityFrameworkCore;

namespace ServiceBricks.Security.Sqlite
{
    /// <summary>
    /// This is the role storage.
    /// </summary>
    public partial class ApplicationRoleStore : GenericApplicationRoleStore<SecuritySqliteContext>
    {
        public ApplicationRoleStore(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IApplicationRoleApiService applicationRoleApiService,
            IApplicationRoleClaimApiService applicationRoleClaimApiService,
            SecuritySqliteContext SecuritySqliteContext,
            IdentityErrorDescriber describer = null) : base(
                mapper,
                businessRuleService,
                applicationRoleApiService,
                applicationRoleClaimApiService,
                SecuritySqliteContext,
                describer)
        {
        }
    }
}