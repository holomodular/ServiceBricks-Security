using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ServiceBricks.Security.EntityFrameworkCore;
using ServiceQuery;

namespace ServiceBricks.Security.SqlServer
{
    /// <summary>
    /// This is the role storage.
    /// </summary>
    public partial class ApplicationRoleStore : GenericApplicationRoleStore<SecuritySqlServerContext>
    {
        protected readonly SecuritySqlServerContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="businessRuleService"></param>
        /// <param name="applicationRoleApiService"></param>
        /// <param name="applicationRoleClaimApiService"></param>
        /// <param name="SecuritySqlServerContext"></param>
        /// <param name="describer"></param>
        public ApplicationRoleStore(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IRoleApiService applicationRoleApiService,
            IRoleClaimApiService applicationRoleClaimApiService,
            SecuritySqlServerContext SecuritySqlServerContext,
            IdentityErrorDescriber describer = null) : base(
                mapper,
                businessRuleService,
                applicationRoleApiService,
                applicationRoleClaimApiService,
                SecuritySqlServerContext,
                describer)
        {
            _context = SecuritySqlServerContext;
        }

        /// <summary>
        /// Query the roles.
        /// </summary>
        public override IQueryable<ApplicationRole> Roles
        {
            get
            {
                return _context.ApplicationRoles.AsQueryable();
            }
        }
    }
}