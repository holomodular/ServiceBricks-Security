using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using ServiceQuery;
using System.Security.Claims;
using ServiceBricks.Security.EntityFrameworkCore;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// This is the role storage.
    /// </summary>
    public abstract class GenericApplicationRoleStore<TContext> : RoleStore<ApplicationRole, TContext, Guid, ApplicationUserRole, ApplicationRoleClaim>
        where TContext : Microsoft.EntityFrameworkCore.DbContext
    {
        protected readonly IMapper _mapper;
        protected readonly IBusinessRuleService _businessRuleService;
        protected readonly IApplicationRoleApiService _applicationRoleApiService;
        protected readonly IApplicationRoleClaimApiService _applicationRoleClaimApiService;

        protected GenericApplicationRoleStore(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IApplicationRoleApiService applicationRoleApiService,
            IApplicationRoleClaimApiService applicationRoleClaimApiService,
            TContext context,
            IdentityErrorDescriber describer = null) : base(context, describer)
        {
            _mapper = mapper;
            _businessRuleService = businessRuleService;
            _applicationRoleApiService = applicationRoleApiService;
            _applicationRoleClaimApiService = applicationRoleClaimApiService;
        }

        public override async Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (role.Id == Guid.Empty)
                role.Id = Guid.NewGuid();
            var roleDto = _mapper.Map<ApplicationRoleDto>(role);
            var resp = await _applicationRoleApiService.CreateAsync(roleDto);
            return resp.GetIdentityResult();
        }

        public override async Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            var roleDto = _mapper.Map<ApplicationRoleDto>(role);
            var resp = await _applicationRoleApiService.DeleteAsync(roleDto.StorageKey);
            return resp.GetIdentityResult();
        }

        public override async Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            var roleDto = _mapper.Map<ApplicationRoleDto>(role);
            var resp = await _applicationRoleApiService.UpdateAsync(roleDto);
            return resp.GetIdentityResult();
        }

        public override async Task AddClaimAsync(ApplicationRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            var item = new ApplicationRoleClaimDto()
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
                RoleStorageKey = role.Id.ToString()
            };
            await _applicationRoleClaimApiService.CreateAsync(item);
        }

        protected override ApplicationRoleClaim CreateRoleClaim(ApplicationRole role, Claim claim)
        {
            var item = new ApplicationRoleClaimDto()
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
                RoleStorageKey = role.Id.ToString()
            };
            var resp = _applicationRoleClaimApiService.Create(item);
            return _mapper.Map<ApplicationRoleClaim>(item);
        }

        public override async Task<ApplicationRole> FindByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var respRole = await _applicationRoleApiService.GetAsync(id);
            if (respRole.Item != null)
                return _mapper.Map<ApplicationRole>(respRole.Item);
            return null;
        }

        public override async Task<ApplicationRole> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationRole.NormalizedName), normalizedName);
            var respQuery = await _applicationRoleApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
                return _mapper.Map<ApplicationRole>(respQuery.Item.List[0]);
            return null;
        }

        public override async Task<System.Collections.Generic.IList<Claim>> GetClaimsAsync(ApplicationRole role, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationRoleClaim.RoleId), role.Id.ToString());
            var respQuery = await _applicationRoleClaimApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
            {
                var roleClaims = _mapper.Map<List<ApplicationRoleClaim>>(respQuery.Item.List);
                return roleClaims.Select(x => x.ToClaim()).ToList();
            }
            return new List<Claim>();
        }

        public override async Task RemoveClaimAsync(ApplicationRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationRoleClaim.RoleId), role.Id.ToString());
            queryBuilder.And();
            queryBuilder.IsEqual(nameof(ApplicationRoleClaim.ClaimType), claim.Type);
            var respQuery = await _applicationRoleClaimApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
                await _applicationRoleClaimApiService.DeleteAsync(respQuery.Item.List[0].StorageKey);
        }

        public override IQueryable<ApplicationRole> Roles
        {
            get
            {
                var respRoles = _applicationRoleApiService.Query(new ServiceQueryRequest());
                var roles = _mapper.Map<List<ApplicationRole>>(respRoles.Item.List);
                return roles.AsQueryable();
            }
        }
    }
}