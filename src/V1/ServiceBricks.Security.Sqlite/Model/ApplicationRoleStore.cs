using Microsoft.AspNetCore.Identity;
using ServiceBricks.Security.EntityFrameworkCore;

namespace ServiceBricks.Security.Sqlite
{
    /// <summary>
    /// This is the role storage.
    /// </summary>
    public partial class ApplicationRoleStore : GenericApplicationRoleStore<SecuritySqliteContext>
    {
        protected readonly SecuritySqliteContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="businessRuleService"></param>
        /// <param name="applicationRoleApiService"></param>
        /// <param name="applicationRoleClaimApiService"></param>
        /// <param name="SecuritySqliteContext"></param>
        /// <param name="describer"></param>
        public ApplicationRoleStore(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IRoleApiService applicationRoleApiService,
            IRoleClaimApiService applicationRoleClaimApiService,
            SecuritySqliteContext SecuritySqliteContext,
            IdentityErrorDescriber describer = null) : base(
                mapper,
                businessRuleService,
                applicationRoleApiService,
                applicationRoleClaimApiService,
                SecuritySqliteContext,
                describer)
        {
            _context = SecuritySqliteContext;
        }

        /// <summary>
        /// Query the roles.
        /// </summary>
        public override IQueryable<ApplicationRole> Roles
        {
            get
            {
                return _context.Roles.AsQueryable();
            }
        }
    }
}