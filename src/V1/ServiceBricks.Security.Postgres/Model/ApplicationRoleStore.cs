using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using ServiceQuery;
using System.Security.Claims;
using ServiceBricks.Security.EntityFrameworkCore;

namespace ServiceBricks.Security.Postgres
{
    /// <summary>
    /// This is the role storage.
    /// </summary>
    public partial class ApplicationRoleStore : GenericApplicationRoleStore<SecurityPostgresContext>
    {
        public ApplicationRoleStore(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IApplicationRoleApiService applicationRoleApiService,
            IApplicationRoleClaimApiService applicationRoleClaimApiService,
            SecurityPostgresContext SecurityPostgresContext,
            IdentityErrorDescriber describer = null) : base(
                mapper,
                businessRuleService,
                applicationRoleApiService,
                applicationRoleClaimApiService,
                SecurityPostgresContext,
                describer)
        {
        }
    }
}