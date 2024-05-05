using AutoMapper;

namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// This is a API service for the ApplicationUserLogin domain object.
    /// </summary>
    public class ApplicationUserLoginApiService : ApiService<ApplicationUserLogin, ApplicationUserLoginDto>, IApplicationUserLoginApiService
    {
        public ApplicationUserLoginApiService(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IDomainRepository<ApplicationUserLogin> repository) : base(mapper, businessRuleService, repository)
        {
        }
    }
}