using AutoMapper;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is a API service for the ApplicationUserRole domain object.
    /// </summary>
    public partial class ApplicationUserRoleApiService : ApiService<ApplicationUserRole, UserRoleDto>, IUserRoleApiService
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="businessRuleService"></param>
        /// <param name="repository"></param>
        public ApplicationUserRoleApiService(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IDomainRepository<ApplicationUserRole> repository) : base(mapper, businessRuleService, repository)
        {
        }
    }
}