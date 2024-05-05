using AutoMapper;


namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is a API service for the ApplicationUserClaim domain object.
    /// </summary>
    public class ApplicationUserClaimApiService : ApiService<ApplicationUserClaim, ApplicationUserClaimDto>, IApplicationUserClaimApiService
    {
        public ApplicationUserClaimApiService(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IDomainRepository<ApplicationUserClaim> repository) : base(mapper, businessRuleService, repository)
        {
        }
    }
}