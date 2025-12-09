namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// This is a API service for the ApplicationUserLogin domain object.
    /// </summary>
    public partial class ApplicationUserLoginApiService : ApiService<ApplicationUserLogin, UserLoginDto>, IUserLoginApiService
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="businessRuleService"></param>
        /// <param name="repository"></param>
        public ApplicationUserLoginApiService(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IDomainRepository<ApplicationUserLogin> repository) : base(mapper, businessRuleService, repository)
        {
        }
    }
}