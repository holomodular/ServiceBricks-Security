using AutoMapper;

namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// This is a API service for the ApplicationUser domain object.
    /// </summary>
    public partial class ApplicationUserApiService : ApiService<ApplicationUser, UserDto>, IUserApiService
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="businessRuleService"></param>
        /// <param name="repositoryApplicationUser"></param>
        public ApplicationUserApiService(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IDomainRepository<ApplicationUser> repositoryApplicationUser)
            : base(mapper, businessRuleService, repositoryApplicationUser)
        {
        }
    }
}