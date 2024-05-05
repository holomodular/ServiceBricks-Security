using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using ServiceQuery;
using System.Security.Claims;
using ServiceBricks.Security.EntityFrameworkCore;

namespace ServiceBricks.Security.Postgres
{
    /// <summary>
    /// This is user storage.
    /// </summary>
    public partial class ApplicationUserStore : GenericApplicationUserStore<SecurityPostgresContext>
    {
        public ApplicationUserStore(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IApplicationUserApiService applicationUserApiService,
            IApplicationUserRoleApiService applicationUserRoleApiService,
            IApplicationUserClaimApiService applicationUserClaimApiService,
            IApplicationUserLoginApiService applicationUserLoginApiService,
            IApplicationUserTokenApiService applicationUserTokenApiService,
            IApplicationRoleApiService applicationRoleApiService,
            SecurityPostgresContext SecurityPostgresContext,
            IdentityErrorDescriber describer = null) : base(
                mapper,
                businessRuleService,
                applicationUserApiService,
                applicationUserRoleApiService,
                applicationUserClaimApiService,
                applicationUserLoginApiService,
                applicationUserTokenApiService,
                applicationRoleApiService,
                SecurityPostgresContext,
                describer)
        {
        }
    }
}