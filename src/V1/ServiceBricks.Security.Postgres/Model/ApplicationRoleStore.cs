using Microsoft.AspNetCore.Identity;
using ServiceBricks.Security.EntityFrameworkCore;

namespace ServiceBricks.Security.Postgres
{
    /// <summary>
    /// This is the role storage.
    /// </summary>
    public partial class ApplicationRoleStore : GenericApplicationRoleStore<SecurityPostgresContext>
    {
        protected readonly SecurityPostgresContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="businessRuleService"></param>
        /// <param name="applicationRoleApiService"></param>
        /// <param name="applicationRoleClaimApiService"></param>
        /// <param name="SecurityPostgresContext"></param>
        /// <param name="describer"></param>
        public ApplicationRoleStore(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IRoleApiService applicationRoleApiService,
            IRoleClaimApiService applicationRoleClaimApiService,
            SecurityPostgresContext SecurityPostgresContext,
            IdentityErrorDescriber describer = null) : base(
                mapper,
                businessRuleService,
                applicationRoleApiService,
                applicationRoleClaimApiService,
                SecurityPostgresContext,
                describer)
        {
            _context = SecurityPostgresContext;
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