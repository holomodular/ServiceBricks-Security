namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// This is a API service for the ApplicationRoleClaim domain object.
    /// </summary>
    public partial class ApplicationRoleClaimApiService : ApiService<ApplicationRoleClaim, RoleClaimDto>, IRoleClaimApiService
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="businessRuleService"></param>
        /// <param name="repository"></param>
        public ApplicationRoleClaimApiService(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IDomainRepository<ApplicationRoleClaim> repository) : base(mapper, businessRuleService, repository)
        {
        }
    }
}