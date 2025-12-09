using Microsoft.AspNetCore.Identity;
using ServiceBricks.Security.EntityFrameworkCore;

namespace ServiceBricks.Security.InMemory
{
    /// <summary>
    /// This is the role storage.
    /// </summary>
    public partial class ApplicationRoleStore : GenericApplicationRoleStore<SecurityInMemoryContext>
    {
        protected readonly SecurityInMemoryContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="businessRuleService"></param>
        /// <param name="applicationRoleApiService"></param>
        /// <param name="applicationRoleClaimApiService"></param>
        /// <param name="securityInMemoryContext"></param>
        /// <param name="describer"></param>
        public ApplicationRoleStore(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IRoleApiService applicationRoleApiService,
            IRoleClaimApiService applicationRoleClaimApiService,
            SecurityInMemoryContext securityInMemoryContext,
            IdentityErrorDescriber describer = null) : base(
                mapper,
                businessRuleService,
                applicationRoleApiService,
                applicationRoleClaimApiService,
                securityInMemoryContext,
                describer)
        {
            _context = securityInMemoryContext;
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