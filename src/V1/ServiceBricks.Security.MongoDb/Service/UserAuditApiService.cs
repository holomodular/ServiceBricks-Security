using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is an API service for the AuditUser domain object.
    /// </summary>
    public partial class UserAuditApiService : ApiService<UserAudit, UserAuditDto>, IUserAuditApiService
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="businessRuleService"></param>
        /// <param name="repository"></param>
        /// <param name="auditUserStorageRepository"></param>
        /// <param name="httpContextAccessor"></param>
        public UserAuditApiService(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IDomainRepository<UserAudit> repository) : base(mapper, businessRuleService, repository)
        {
        }
    }
}