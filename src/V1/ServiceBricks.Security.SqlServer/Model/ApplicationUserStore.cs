using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ServiceBricks.Security.EntityFrameworkCore;
using ServiceQuery;

namespace ServiceBricks.Security.SqlServer
{
    /// <summary>
    /// This is user storage.
    /// </summary>
    public partial class ApplicationUserStore : GenericApplicationUserStore<SecuritySqlServerContext>
    {
        protected readonly SecuritySqlServerContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="businessRuleService"></param>
        /// <param name="applicationUserApiService"></param>
        /// <param name="applicationUserRoleApiService"></param>
        /// <param name="applicationUserClaimApiService"></param>
        /// <param name="applicationUserLoginApiService"></param>
        /// <param name="applicationUserTokenApiService"></param>
        /// <param name="applicationRoleApiService"></param>
        /// <param name="SecuritySqlServerContext"></param>
        /// <param name="describer"></param>
        public ApplicationUserStore(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IApplicationUserApiService applicationUserApiService,
            IApplicationUserRoleApiService applicationUserRoleApiService,
            IApplicationUserClaimApiService applicationUserClaimApiService,
            IApplicationUserLoginApiService applicationUserLoginApiService,
            IApplicationUserTokenApiService applicationUserTokenApiService,
            IApplicationRoleApiService applicationRoleApiService,
            SecuritySqlServerContext SecuritySqlServerContext,
            IdentityErrorDescriber describer = null) : base(
                mapper,
                businessRuleService,
                applicationUserApiService,
                applicationUserRoleApiService,
                applicationUserClaimApiService,
                applicationUserLoginApiService,
                applicationUserTokenApiService,
                applicationRoleApiService,
                SecuritySqlServerContext,
                describer)
        {
            _context = SecuritySqlServerContext;
        }

        /// <summary>
        /// Query users.
        /// </summary>
        public override IQueryable<ApplicationUser> Users
        {
            get
            {
                return _context.ApplicationUsers.AsQueryable();
            }
        }
    }
}