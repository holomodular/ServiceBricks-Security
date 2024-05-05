using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using ServiceQuery;
using System.Security.Claims;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// This is user storage.
    /// </summary>
    public abstract class GenericApplicationUserStore<TContext> : UserStore<ApplicationUser, ApplicationRole, TContext, Guid, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationUserToken, ApplicationRoleClaim>
        where TContext : Microsoft.EntityFrameworkCore.DbContext
    {
        protected readonly IMapper _mapper;
        protected readonly IBusinessRuleService _businessRuleService;
        protected readonly IApplicationUserApiService _applicationUserApiService;
        protected readonly IApplicationUserRoleApiService _applicationUserRoleApiService;
        protected readonly IApplicationUserClaimApiService _applicationUserClaimApiService;
        protected readonly IApplicationUserLoginApiService _applicationUserLoginApiService;
        protected readonly IApplicationUserTokenApiService _applicationUserTokenApiService;
        protected readonly IApplicationRoleApiService _applicationRoleApiService;

        protected GenericApplicationUserStore(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IApplicationUserApiService applicationUserApiService,
            IApplicationUserRoleApiService applicationUserRoleApiService,
            IApplicationUserClaimApiService applicationUserClaimApiService,
            IApplicationUserLoginApiService applicationUserLoginApiService,
            IApplicationUserTokenApiService applicationUserTokenApiService,
            IApplicationRoleApiService applicationRoleApiService,
            TContext context,
            IdentityErrorDescriber describer = null) : base(context, describer)
        {
            _mapper = mapper;
            _businessRuleService = businessRuleService;
            _applicationUserApiService = applicationUserApiService;
            _applicationUserRoleApiService = applicationUserRoleApiService;
            _applicationUserClaimApiService = applicationUserClaimApiService;
            _applicationUserLoginApiService = applicationUserLoginApiService;
            _applicationUserTokenApiService = applicationUserTokenApiService;
            _applicationRoleApiService = applicationRoleApiService;
        }

        public override async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (user.Id == Guid.Empty)
                user.Id = Guid.NewGuid();
            var userDto = _mapper.Map<ApplicationUserDto>(user);
            var resp = await _applicationUserApiService.CreateAsync(userDto);
            return resp.GetIdentityResult();
        }

        public override async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            var userDto = _mapper.Map<ApplicationUserDto>(user);
            var resp = await _applicationUserApiService.UpdateAsync(userDto);
            return resp.GetIdentityResult();
        }

        public override async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            var userDto = _mapper.Map<ApplicationUserDto>(user);
            var resp = await _applicationUserApiService.DeleteAsync(userDto.StorageKey);
            return resp.GetIdentityResult();
        }

        public override async Task AddClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            foreach (var claim in claims)
            {
                var uc = new ApplicationUserClaimDto();
                uc.ClaimType = claim.Type;
                uc.ClaimValue = claim.Value;
                uc.UserStorageKey = user.Id.ToString();
                _ = await _applicationUserClaimApiService.CreateAsync(uc);
            }
        }

        protected override ApplicationUserClaim CreateUserClaim(ApplicationUser user, Claim claim)
        {
            var uc = new ApplicationUserClaimDto();
            uc.ClaimType = claim.Type;
            uc.ClaimValue = claim.Value;
            uc.UserStorageKey = user.Id.ToString();
            var resp = _applicationUserClaimApiService.Create(uc);
            if (resp.Success)
                return _mapper.Map<ApplicationUserClaim>(uc);
            return null;
        }

        public override async Task<System.Collections.Generic.IList<Claim>> GetClaimsAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationUserClaim.UserId), user.Id.ToString());
            var respUserClaims = await _applicationUserClaimApiService.QueryAsync(queryBuilder.Build());
            if (respUserClaims.Success && respUserClaims.Item.List.Count > 0)
            {
                var userClaims = _mapper.Map<List<ApplicationUserClaim>>(respUserClaims.Item.List);
                return userClaims.Select(x => x.ToClaim()).ToList();
            }
            return new List<Claim>();
        }

        public override async Task AddToRoleAsync(ApplicationUser user, string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationRole.NormalizedName), normalizedRoleName);
            var respRole = await _applicationRoleApiService.QueryAsync(queryBuilder.Build());
            if (respRole.Success && respRole.Item.List.Count > 0)
            {
                var role = respRole.Item.List[0];
                var userRole = new ApplicationUserRoleDto()
                {
                    UserStorageKey = user.Id.ToString(),
                    RoleStorageKey = role.StorageKey
                };
                await _applicationUserRoleApiService.CreateAsync(userRole);
            }
        }

        public override async Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            var respUser = await _applicationUserApiService.GetAsync(userId);
            if (respUser.Item != null)
                return _mapper.Map<ApplicationUser>(respUser.Item);
            return null;
        }

        public override async Task<ApplicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationUser.NormalizedEmail), normalizedEmail);
            var respQuery = await _applicationUserApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
                return _mapper.Map<ApplicationUser>(respQuery.Item.List[0]);
            return null;
        }

        public override async Task RemoveClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            if (claims != null && claims.Count() > 0)
            {
                ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
                queryBuilder.IsEqual(nameof(ApplicationUserClaim.UserId), user.Id.ToString());
                var respQuery = await _applicationUserClaimApiService.QueryAsync(queryBuilder.Build());
                if (respQuery.Success && respQuery.Item.List.Count > 0)
                {
                    var existingClaims = respQuery.Item.List;
                    foreach (var claim in claims)
                    {
                        var found = existingClaims.Where(x => x.ClaimType == claim.Type).FirstOrDefault();
                        if (found != null)
                            _ = await _applicationUserClaimApiService.DeleteAsync(found.StorageKey);
                    }
                }
            }
        }

        public override async Task<System.Collections.Generic.IList<ApplicationUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationUserClaim.ClaimType), claim.Type);
            var respUserClaims = await _applicationUserClaimApiService.QueryAsync(queryBuilder.Build());

            queryBuilder = new ServiceQueryRequestBuilder();
            var respUsers = await _applicationUserApiService.QueryAsync(queryBuilder.Build());
            var users = _mapper.Map<List<ApplicationUser>>(respUsers.Item.List);
            return users;
        }

        public override async Task ReplaceClaimAsync(ApplicationUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationUserClaim.UserId), user.Id.ToString());
            queryBuilder.And();
            queryBuilder.IsEqual(nameof(ApplicationUserClaim.ClaimType), claim.Type);
            var respQuery = await _applicationUserClaimApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
            {
                var claimDto = respQuery.Item.List[0];
                claimDto.ClaimType = newClaim.Type;
                claimDto.ClaimValue = newClaim.Value;
                await _applicationUserClaimApiService.UpdateAsync(claimDto);
            }
        }

        protected override ApplicationUserRole CreateUserRole(ApplicationUser user, ApplicationRole role)
        {
            var item = new ApplicationUserRoleDto()
            {
                UserStorageKey = user.Id.ToString(),
                RoleStorageKey = role.Id.ToString()
            };
            var resp = _applicationUserRoleApiService.Create(item);
            return _mapper.Map<ApplicationUserRole>(item);
        }

        protected override async Task<ApplicationRole> FindRoleAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationRole.NormalizedName), normalizedRoleName);
            var respQuery = await _applicationRoleApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
                return _mapper.Map<ApplicationRole>(respQuery.Item.List[0]);
            return null;
        }

        protected override async Task<ApplicationUserRole> FindUserRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationUserRole.UserId), userId.ToString());
            queryBuilder.And();
            queryBuilder.IsEqual(nameof(ApplicationUserRole.RoleId), roleId.ToString());
            var respQuery = await _applicationUserRoleApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
                return _mapper.Map<ApplicationUserRole>(respQuery.Item.List[0]);
            return null;
        }

        public override async Task<System.Collections.Generic.IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationUserRole.UserId), user.Id.ToString());
            var respQuery = await _applicationUserRoleApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
            {
                var roleIds = respQuery.Item.List.Select(x => x.RoleStorageKey.ToString()).ToArray();
                queryBuilder = new ServiceQueryRequestBuilder();
                queryBuilder.IsInSet(nameof(ApplicationRole.Id), roleIds);
                var respRoles = await _applicationRoleApiService.QueryAsync(queryBuilder.Build());
                if (respRoles.Success && respRoles.Item.List.Count > 0)
                    return respRoles.Item.List.Select(x => x.Name).ToList();
            }
            return new List<string>();
        }

        public override async Task<System.Collections.Generic.IList<ApplicationUser>> GetUsersInRoleAsync(string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationRole.NormalizedName), normalizedRoleName);
            var respRoles = await _applicationRoleApiService.QueryAsync(queryBuilder.Build());
            if (respRoles.Success && respRoles.Item.List.Count > 0)
            {
                var role = respRoles.Item.List[0];
                queryBuilder = new ServiceQueryRequestBuilder();
                queryBuilder.IsEqual(nameof(ApplicationUserRole.RoleId), role.StorageKey);
                var respUserRoles = await _applicationUserRoleApiService.QueryAsync(queryBuilder.Build());
                if (respUserRoles.Success && respUserRoles.Item.List.Count > 0)
                {
                    var userIds = respUserRoles.Item.List.Select(x => x.UserStorageKey.ToString()).ToArray();
                    queryBuilder = new ServiceQueryRequestBuilder();
                    queryBuilder.IsInSet(nameof(ApplicationUser.Id), userIds);
                    var respUsers = await _applicationUserApiService.QueryAsync(queryBuilder.Build());
                    if (respUsers.Success && respUsers.Item.List.Count > 0)
                        return _mapper.Map<List<ApplicationUser>>(respUsers.Item.List);
                }
            }
            return new List<ApplicationUser>();
        }

        public override async Task<bool> IsInRoleAsync(ApplicationUser user, string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationRole.NormalizedName), normalizedRoleName);
            var respRoles = await _applicationRoleApiService.QueryAsync(queryBuilder.Build());
            if (respRoles.Success && respRoles.Item.List.Count > 0)
            {
                var role = respRoles.Item.List[0];
                queryBuilder = new ServiceQueryRequestBuilder();
                queryBuilder.IsEqual(nameof(ApplicationUserRole.RoleId), role.StorageKey);
                queryBuilder.And();
                queryBuilder.IsEqual(nameof(ApplicationUserRole.UserId), user.Id.ToString());
                var respUserRoles = await _applicationUserRoleApiService.QueryAsync(queryBuilder.Build());
                if (respUserRoles.Success && respUserRoles.Item.List.Count > 0)
                    return true;
            }
            return false;
        }

        public override async Task RemoveFromRoleAsync(ApplicationUser user, string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationRole.NormalizedName), normalizedRoleName);
            var respRoles = await _applicationRoleApiService.QueryAsync(queryBuilder.Build());
            if (respRoles.Success && respRoles.Item.List.Count > 0)
            {
                var role = respRoles.Item.List[0];
                queryBuilder = new ServiceQueryRequestBuilder();
                queryBuilder.IsEqual(nameof(ApplicationUserRole.RoleId), role.StorageKey);
                queryBuilder.And();
                queryBuilder.IsEqual(nameof(ApplicationUserRole.UserId), user.Id.ToString());
                var respUserRoles = await _applicationUserRoleApiService.QueryAsync(queryBuilder.Build());
                if (respUserRoles.Success && respUserRoles.Item.List.Count > 0)
                    await _applicationUserRoleApiService.DeleteAsync(respUserRoles.Item.List[0].StorageKey);
            }
        }

        public override async Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationUser.NormalizedUserName), normalizedUserName);
            var respQuery = await _applicationUserApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
                return _mapper.Map<ApplicationUser>(respQuery.Item.List[0]);
            return null;
        }

        protected override async Task<ApplicationUser> FindUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            var respUser = await _applicationUserApiService.GetAsync(userId.ToString());
            return _mapper.Map<ApplicationUser>(respUser.Item);
        }

        protected override ApplicationUserLogin CreateUserLogin(ApplicationUser user, UserLoginInfo login)
        {
            var ul = new ApplicationUserLoginDto()
            {
                LoginProvider = login.LoginProvider,
                ProviderDisplayName = login.ProviderDisplayName,
                ProviderKey = login.ProviderKey,
                UserStorageKey = user.Id.ToString(),
            };
            var resp = _applicationUserLoginApiService.Create(ul);
            if (resp.Success)
                return _mapper.Map<ApplicationUserLogin>(resp.Item);
            return null;
        }

        protected override ApplicationUserToken CreateUserToken(ApplicationUser user, string loginProvider, string name, string value)
        {
            var ut = new ApplicationUserTokenDto()
            {
                LoginProvider = loginProvider,
                Name = name,
                UserStorageKey = user.Id.ToString(),
                Value = value,
            };
            var resp = _applicationUserTokenApiService.Create(ut);
            if (resp.Success)
                return _mapper.Map<ApplicationUserToken>(ut);
            return null;
        }

        public override IQueryable<ApplicationUser> Users
        {
            get
            {
                var respUsers = _applicationUserApiService.Query(new ServiceQueryRequest());
                var users = _mapper.Map<List<ApplicationUser>>(respUsers.Item.List);
                return users.AsQueryable();
            }
        }
    }
}