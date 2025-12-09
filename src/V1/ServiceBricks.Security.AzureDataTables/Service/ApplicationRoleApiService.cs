namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is a API service for the ApplicationRole domain object.
    /// </summary>
    public partial class ApplicationRoleApiService : ApiService<ApplicationRole, RoleDto>, IRoleApiService
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="businessRuleService"></param>
        /// <param name="repository"></param>
        public ApplicationRoleApiService(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IDomainRepository<ApplicationRole> repository) : base(mapper, businessRuleService, repository)
        {
        }
    }
}