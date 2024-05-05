using AutoMapper;
using Microsoft.AspNetCore.Http;

using ServiceQuery;

namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// This is an API service for the AuditUser domain object.
    /// </summary>
    public class AuditUserApiService : ApiService<AuditUser, AuditUserDto>, IAuditUserApiService
    {
        protected readonly IAuditUserStorageRepository _auditUserStorageRepository;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public AuditUserApiService(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IDomainRepository<AuditUser> repository,
            IAuditUserStorageRepository auditUserStorageRepository,
            IHttpContextAccessor httpContextAccessor) : base(mapper, businessRuleService, repository)
        {
            _auditUserStorageRepository = auditUserStorageRepository;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}