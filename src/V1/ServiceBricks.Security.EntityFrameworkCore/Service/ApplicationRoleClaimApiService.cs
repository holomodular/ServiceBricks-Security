using AutoMapper;


namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// This is a API service for the ApplicationRoleClaim domain object.
    /// </summary>
    public class ApplicationRoleClaimApiService : ApiService<ApplicationRoleClaim, ApplicationRoleClaimDto>, IApplicationRoleClaimApiService
    {
        public ApplicationRoleClaimApiService(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IDomainRepository<ApplicationRoleClaim> repository) : base(mapper, businessRuleService, repository)
        {
        }
    }
}