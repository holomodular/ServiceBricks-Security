using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ServiceBricks.Security.EntityFrameworkCore
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