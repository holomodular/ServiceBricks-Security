using AutoMapper;
using Microsoft.AspNetCore.Identity;

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
        protected readonly IApplicationRoleApiService _applicationRoleApiService;
        protected readonly IApplicationRoleClaimApiService _applicationRoleClaimApiService;

        public ApplicationRoleStore(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IApplicationRoleApiService applicationRoleApiService,
            IApplicationRoleClaimApiService applicationRoleClaimApiService,
            IdentityErrorDescriber describer = null) : base(describer)
        {
            _mapper = mapper;
            _businessRuleService = businessRuleService;
            _applicationRoleApiService = applicationRoleApiService;
            _applicationRoleClaimApiService = applicationRoleClaimApiService;
        }

        public override async Task<IdentityResult> CreateAsync(ApplicationIdentityRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            var roleDto = _mapper.Map<ApplicationRoleDto>(role);
            var resp = await _applicationRoleApiService.CreateAsync(roleDto);
            return resp.GetIdentityResult();
        }

        public override async Task<IdentityResult> DeleteAsync(ApplicationIdentityRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            var roleDto = _mapper.Map<ApplicationRoleDto>(role);
            var resp = await _applicationRoleApiService.DeleteAsync(roleDto.StorageKey);
            return resp.GetIdentityResult();
        }

        public override async Task<IdentityResult> UpdateAsync(ApplicationIdentityRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            var roleDto = _mapper.Map<ApplicationRoleDto>(role);
            var resp = await _applicationRoleApiService.UpdateAsync(roleDto);
            return resp.GetIdentityResult();
        }

        public override async Task AddClaimAsync(ApplicationIdentityRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            var item = new ApplicationRoleClaimDto()
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
                RoleStorageKey = role.Id.ToString()
            };
            await _applicationRoleClaimApiService.CreateAsync(item);
        }

        protected override ApplicationIdentityRoleClaim CreateRoleClaim(ApplicationIdentityRole role, Claim claim)
        {
            var item = new ApplicationRoleClaimDto()
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
                RoleStorageKey = role.Id.ToString()
            };
            var resp = _applicationRoleClaimApiService.Create(item);
            return _mapper.Map<ApplicationIdentityRoleClaim>(item);
        }

        public override async Task<ApplicationIdentityRole> FindByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var respRole = await _applicationRoleApiService.GetAsync(id);
            if (respRole.Item != null)
                return _mapper.Map<ApplicationIdentityRole>(respRole.Item);
            return null;
        }

        public override async Task<ApplicationIdentityRole> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationIdentityRole.NormalizedName), normalizedName);
            var respQuery = await _applicationRoleApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
                return _mapper.Map<ApplicationIdentityRole>(respQuery.Item.List[0]);
            return null;
        }

        public override async Task<System.Collections.Generic.IList<Claim>> GetClaimsAsync(ApplicationIdentityRole role, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationIdentityRoleClaim.RoleId), role.Id.ToString());
            var respQuery = await _applicationRoleClaimApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
            {
                var roleClaims = _mapper.Map<List<ApplicationIdentityRoleClaim>>(respQuery.Item.List);
                return roleClaims.Select(x => x.ToClaim()).ToList();
            }
            return new List<Claim>();
        }

        public override async Task RemoveClaimAsync(ApplicationIdentityRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationIdentityRoleClaim.RoleId), role.Id.ToString());
            queryBuilder.And();
            queryBuilder.IsEqual(nameof(ApplicationIdentityRoleClaim.ClaimType), claim.Type);
            var respQuery = await _applicationRoleClaimApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
                await _applicationRoleClaimApiService.DeleteAsync(respQuery.Item.List[0].StorageKey);
        }

        public override IQueryable<ApplicationIdentityRole> Roles
        {
            get
            {
                var respRoles = _applicationRoleApiService.Query(new ServiceQueryRequest());
                var roles = _mapper.Map<List<ApplicationIdentityRole>>(respRoles.Item.List);
                return roles.AsQueryable();
            }
        }
    }
}