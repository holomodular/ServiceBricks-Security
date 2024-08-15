using AutoMapper;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// This is a API service for the ApplicationUserToken domain object.
    /// </summary>
    public partial class ApplicationUserTokenApiService : ApiService<ApplicationUserToken, UserTokenDto>, IUserTokenApiService
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="businessRuleService"></param>
        /// <param name="repository"></param>
        public ApplicationUserTokenApiService(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IDomainRepository<ApplicationUserToken> repository) : base(mapper, businessRuleService, repository)
        {
        }
    }
}