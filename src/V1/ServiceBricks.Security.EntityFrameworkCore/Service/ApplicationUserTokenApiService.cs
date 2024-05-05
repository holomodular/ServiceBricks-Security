using AutoMapper;


namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// This is a API service for the ApplicationUserToken domain object.
    /// </summary>
    public class ApplicationUserTokenApiService : ApiService<ApplicationUserToken, ApplicationUserTokenDto>, IApplicationUserTokenApiService
    {
        public ApplicationUserTokenApiService(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IDomainRepository<ApplicationUserToken> repository) : base(mapper, businessRuleService, repository)
        {
        }
    }
}