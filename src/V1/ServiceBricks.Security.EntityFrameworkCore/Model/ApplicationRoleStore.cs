using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ServiceQuery;
using System.Security.Claims;

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
        protected readonly IRoleApiService _applicationRoleApiService;
        protected readonly IRoleClaimApiService _applicationRoleClaimApiService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="businessRuleService"></param>
        /// <param name="applicationRoleApiService"></param>
        /// <param name="applicationRoleClaimApiService"></param>
        /// <param name="context"></param>
        /// <param name="describer"></param>
        public GenericApplicationRoleStore(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IRoleApiService applicationRoleApiService,
            IRoleClaimApiService applicationRoleClaimApiService,
            TContext context,
            IdentityErrorDescriber describer = null) : base(context, describer)
        {
            _mapper = mapper;
            _businessRuleService = businessRuleService;
            _applicationRoleApiService = applicationRoleApiService;
            _applicationRoleClaimApiService = applicationRoleClaimApiService;
        }

        /// <summary>
        /// Create a new role.
        /// </summary>
        /// <param name="role"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (role.Id == Guid.Empty)
                role.Id = Guid.NewGuid();
            var roleDto = _mapper.Map<RoleDto>(role);
            var resp = await _applicationRoleApiService.CreateAsync(roleDto);
            return resp.GetIdentityResult();
        }

        /// <summary>
        /// Delete a role.
        /// </summary>
        /// <param name="role"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            var roleDto = _mapper.Map<RoleDto>(role);
            var resp = await _applicationRoleApiService.DeleteAsync(roleDto.StorageKey);
            return resp.GetIdentityResult();
        }

        /// <summary>
        /// Update a role.
        /// </summary>
        /// <param name="role"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            var roleDto = _mapper.Map<RoleDto>(role);
            var resp = await _applicationRoleApiService.UpdateAsync(roleDto);
            return resp.GetIdentityResult();
        }

        /// <summary>
        /// Add a claim to a role.
        /// </summary>
        /// <param name="role"></param>
        /// <param name="claim"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task AddClaimAsync(ApplicationRole role, Claim claim, CancellationToken cancellationToken = default)
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
        /// Create a role claim.
        /// </summary>
        /// <param name="role"></param>
        /// <param name="claim"></param>
        /// <returns></returns>
        protected override ApplicationRoleClaim CreateRoleClaim(ApplicationRole role, Claim claim)
        {
            var item = new RoleClaimDto()
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
                RoleStorageKey = role.Id.ToString()
            };
            var resp = _applicationRoleClaimApiService.Create(item);
            return _mapper.Map<ApplicationRoleClaim>(item);
        }

        /// <summary>
        /// Find a role by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<ApplicationRole> FindByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var respRole = await _applicationRoleApiService.GetAsync(id);
            if (respRole.Item != null)
                return _mapper.Map<ApplicationRole>(respRole.Item);
            return null;
        }

        /// <summary>
        /// Find a role by name.
        /// </summary>
        /// <param name="normalizedName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<ApplicationRole> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(RoleDto.NormalizedName), normalizedName);
            var respQuery = await _applicationRoleApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
                return _mapper.Map<ApplicationRole>(respQuery.Item.List[0]);
            return null;
        }

        /// <summary>
        /// Get the claims for a role.
        /// </summary>
        /// <param name="role"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<System.Collections.Generic.IList<Claim>> GetClaimsAsync(ApplicationRole role, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(RoleClaimDto.RoleStorageKey), role.Id.ToString());
            var respQuery = await _applicationRoleClaimApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
            {
                var roleClaims = _mapper.Map<List<ApplicationRoleClaim>>(respQuery.Item.List);
                return roleClaims.Select(x => x.ToClaim()).ToList();
            }
            return new List<Claim>();
        }

        /// <summary>
        /// Remove a claim from a role.
        /// </summary>
        /// <param name="role"></param>
        /// <param name="claim"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task RemoveClaimAsync(ApplicationRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(RoleClaimDto.RoleStorageKey), role.Id.ToString());
            queryBuilder.And();
            queryBuilder.IsEqual(nameof(RoleClaimDto.ClaimType), claim.Type);
            var respQuery = await _applicationRoleClaimApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
                await _applicationRoleClaimApiService.DeleteAsync(respQuery.Item.List[0].StorageKey);
        }

        /// <summary>
        /// Query the roles.
        /// </summary>
        public override IQueryable<ApplicationRole> Roles
        {
            get
            {
                // Not supported
                return new List<ApplicationRole>().AsQueryable();
            }
        }
    }
}