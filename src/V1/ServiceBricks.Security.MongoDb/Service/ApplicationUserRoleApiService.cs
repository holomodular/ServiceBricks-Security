using AutoMapper;


namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is a API service for the ApplicationUserRole domain object.
    /// </summary>
    public class ApplicationUserRoleApiService : ApiService<ApplicationUserRole, ApplicationUserRoleDto>, IApplicationUserRoleApiService
    {
        public ApplicationUserRoleApiService(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IDomainRepository<ApplicationUserRole> repository) : base(mapper, businessRuleService, repository)
        {
        }
    }
}