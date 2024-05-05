using AutoMapper;


namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is a API service for the ApplicationRole domain object.
    /// </summary>
    public class ApplicationRoleApiService : ApiService<ApplicationRole, ApplicationRoleDto>, IApplicationRoleApiService
    {
        public ApplicationRoleApiService(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IDomainRepository<ApplicationRole> repository) : base(mapper, businessRuleService, repository)
        {
        }
    }
}