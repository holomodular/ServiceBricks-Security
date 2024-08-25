using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceBricks.Security.EntityFrameworkCore;

namespace ServiceBricks.Security.Sqlite
{
    /// <summary>
    /// This is user storage.
    /// </summary>
    public partial class ApplicationUserStore : GenericApplicationUserStore<SecuritySqliteContext>
    {
        protected readonly SecuritySqliteContext _context;

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
        /// <param name="SecuritySqliteContext"></param>
        /// <param name="describer"></param>
        public ApplicationUserStore(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IUserApiService applicationUserApiService,
            IUserRoleApiService applicationUserRoleApiService,
            IUserClaimApiService applicationUserClaimApiService,
            IUserLoginApiService applicationUserLoginApiService,
            IUserTokenApiService applicationUserTokenApiService,
            IRoleApiService applicationRoleApiService,
            SecuritySqliteContext SecuritySqliteContext,
            IdentityErrorDescriber describer = null) : base(
                mapper,
                businessRuleService,
                applicationUserApiService,
                applicationUserRoleApiService,
                applicationUserClaimApiService,
                applicationUserLoginApiService,
                applicationUserTokenApiService,
                applicationRoleApiService,
                SecuritySqliteContext,
                describer)
        {
            _context = SecuritySqliteContext;
        }

        /// <summary>
        /// Query users.
        /// </summary>
        public override IQueryable<ApplicationUser> Users
        {
            get
            {
                return _context.Users.AsQueryable();
            }
        }
    }
}