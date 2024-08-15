using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using ServiceQuery;
using System.Security.Claims;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is the role storage.
    /// </summary>
    public partial class ApplicationRoleStore : RoleStoreBase<ApplicationIdentityRole, string, ApplicationUserRole, ApplicationIdentityRoleClaim>
    {
        protected readonly IMapper _mapper;
        protected readonly IBusinessRuleService _businessRuleService;
        protected readonly IRoleApiService _applicationRoleApiService;
        protected readonly IRoleClaimApiService _applicationRoleClaimApiService;
        protected readonly SecurityStorageRepository<ApplicationRole> _securityStorageRepositoryApplicationRole;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="businessRuleService"></param>
        /// <param name="applicationRoleApiService"></param>
        /// <param name="applicationRoleClaimApiService"></param>
        /// <param name="describer"></param>
        public ApplicationRoleStore(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IRoleApiService applicationRoleApiService,
            IRoleClaimApiService applicationRoleClaimApiService,
            SecurityStorageRepository<ApplicationRole> securityStorageRepositoryApplicationRole,
            IdentityErrorDescriber describer = null) : base(describer)
        {
            _mapper = mapper;
            _businessRuleService = businessRuleService;
            _applicationRoleApiService = applicationRoleApiService;
            _applicationRoleClaimApiService = applicationRoleClaimApiService;
            _securityStorageRepositoryApplicationRole = securityStorageRepositoryApplicationRole;
        }

        /// <summary>
        /// Create a role
        /// </summary>
        /// <param name="role"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<IdentityResult> CreateAsync(ApplicationIdentityRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            var roleDto = _mapper.Map<RoleDto>(role);
            var resp = await _applicationRoleApiService.CreateAsync(roleDto);
            return resp.GetIdentityResult();
        }

        /// <summary>
        /// Delete a role
        /// </summary>
        /// <param name="role"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<IdentityResult> DeleteAsync(ApplicationIdentityRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            var roleDto = _mapper.Map<RoleDto>(role);
            var resp = await _applicationRoleApiService.DeleteAsync(roleDto.StorageKey);
            return resp.GetIdentityResult();
        }

        /// <summary>
        /// Update a role
        /// </summary>
        /// <param name="role"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<IdentityResult> UpdateAsync(ApplicationIdentityRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            var roleDto = _mapper.Map<RoleDto>(role);
            var resp = await _applicationRoleApiService.UpdateAsync(roleDto);
            return resp.GetIdentityResult();
        }

        /// <summary>
        /// Add a claim to a role
        /// </summary>
        /// <param name="role"></param>
        /// <param name="claim"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task AddClaimAsync(ApplicationIdentityRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            var item = new RoleClaimDto()
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
                RoleStorageKey = role.Id.ToString()
            };
            await _applicationRoleClaimApiService.CreateAsync(item);
        }

        /// <summary>
        /// Create a role claim
        /// </summary>
        /// <param name="role"></param>
        /// <param name="claim"></param>
        /// <returns></returns>
        protected override ApplicationIdentityRoleClaim CreateRoleClaim(ApplicationIdentityRole role, Claim claim)
        {
            var item = new RoleClaimDto()
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
                RoleStorageKey = role.Id.ToString()
            };
            var resp = _applicationRoleClaimApiService.Create(item);
            return _mapper.Map<ApplicationIdentityRoleClaim>(item);
        }

        /// <summary>
        /// Find a role by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<ApplicationIdentityRole> FindByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var respRole = await _applicationRoleApiService.GetAsync(id);
            if (respRole.Item != null)
                return _mapper.Map<ApplicationIdentityRole>(respRole.Item);
            return null;
        }

        /// <summary>
        /// Find a role by normalized name
        /// </summary>
        /// <param name="normalizedName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<ApplicationIdentityRole> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(RoleDto.NormalizedName), normalizedName);
            var respQuery = await _applicationRoleApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
                return _mapper.Map<ApplicationIdentityRole>(respQuery.Item.List[0]);
            return null;
        }

        /// <summary>
        /// Get claims for a role
        /// </summary>
        /// <param name="role"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<System.Collections.Generic.IList<Claim>> GetClaimsAsync(ApplicationIdentityRole role, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(RoleClaimDto.RoleStorageKey), role.Id);
            var respQuery = await _applicationRoleClaimApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
            {
                var roleClaims = _mapper.Map<List<ApplicationIdentityRoleClaim>>(respQuery.Item.List);
                return roleClaims.Select(x => x.ToClaim()).ToList();
            }
            return new List<Claim>();
        }

        /// <summary>
        /// Remove a claim from a role
        /// </summary>
        /// <param name="role"></param>
        /// <param name="claim"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task RemoveClaimAsync(ApplicationIdentityRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(RoleClaimDto.RoleStorageKey), role.Id);
            queryBuilder.And();
            queryBuilder.IsEqual(nameof(RoleClaimDto.ClaimType), claim.Type);
            var respQuery = await _applicationRoleClaimApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
                await _applicationRoleClaimApiService.DeleteAsync(respQuery.Item.List[0].StorageKey);
        }

        /// <summary>
        /// Query roles
        /// </summary>
        public override IQueryable<ApplicationIdentityRole> Roles
        {
            get
            {
                MongoClient client = new MongoClient(_securityStorageRepositoryApplicationRole.ConnectionString);
                var db = client.GetDatabase(_securityStorageRepositoryApplicationRole.DatabaseName);
                var collection = db.GetCollection<ApplicationIdentityRole>(_securityStorageRepositoryApplicationRole.CollectionName);
                return collection.AsQueryable();
            }
        }
    }
}