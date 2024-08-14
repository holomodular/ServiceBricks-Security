using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is an API service for the AuditUser domain object.
    /// </summary>
    public partial class AuditUserApiService : ApiService<AuditUser, AuditUserDto>, IAuditUserApiService
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="businessRuleService"></param>
        /// <param name="repository"></param>
        /// <param name="auditUserStorageRepository"></param>
        /// <param name="httpContextAccessor"></param>
        public AuditUserApiService(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IDomainRepository<AuditUser> repository) : base(mapper, businessRuleService, repository)
        {
        }
    }
}