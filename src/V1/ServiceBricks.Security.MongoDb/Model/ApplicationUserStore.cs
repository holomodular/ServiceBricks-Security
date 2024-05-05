using AutoMapper;
using Microsoft.AspNetCore.Identity;

using ServiceBricks.Storage.MongoDb;
using ServiceQuery;
using System.Security.Claims;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is user storage.
    /// </summary>
    public partial class ApplicationUserStore : UserStoreBase<ApplicationIdentityUser, ApplicationIdentityRole, string, ApplicationIdentityUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationUserToken, ApplicationIdentityRoleClaim>
    {
        protected readonly IMapper _mapper;
        protected readonly IBusinessRuleService _businessRuleService;
        protected readonly IApplicationUserApiService _applicationUserApiService;
        protected readonly IApplicationUserRoleApiService _applicationUserRoleApiService;
        protected readonly IApplicationUserClaimApiService _applicationUserClaimApiService;
        protected readonly IApplicationUserLoginApiService _applicationUserLoginApiService;
        protected readonly IApplicationUserTokenApiService _applicationUserTokenApiService;
        protected readonly IApplicationRoleApiService _applicationRoleApiService;

        public ApplicationUserStore(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IApplicationUserApiService applicationUserApiService,
            IApplicationUserRoleApiService applicationUserRoleApiService,
            IApplicationUserClaimApiService applicationUserClaimApiService,
            IApplicationUserLoginApiService applicationUserLoginApiService,
            IApplicationUserTokenApiService applicationUserTokenApiService,
            IApplicationRoleApiService applicationRoleApiService,
            IdentityErrorDescriber describer = null) : base(describer)
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

        public override async Task<IdentityResult> CreateAsync(ApplicationIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            var userDto = _mapper.Map<ApplicationUserDto>(user);
            var resp = await _applicationUserApiService.CreateAsync(userDto);
            if (resp.Success)
                _mapper.Map<ApplicationUserDto, ApplicationIdentityUser>(resp.Item, user);
            return resp.GetIdentityResult();
        }

        public override async Task<IdentityResult> UpdateAsync(ApplicationIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            var userDto = _mapper.Map<ApplicationUserDto>(user);
            var resp = await _applicationUserApiService.UpdateAsync(userDto);
            return resp.GetIdentityResult();
        }

        public override async Task<IdentityResult> DeleteAsync(ApplicationIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            var userDto = _mapper.Map<ApplicationUserDto>(user);
            var resp = await _applicationUserApiService.DeleteAsync(userDto.StorageKey);
            return resp.GetIdentityResult();
        }

        public override async Task AddClaimsAsync(ApplicationIdentityUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
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

        protected override ApplicationIdentityUserClaim CreateUserClaim(ApplicationIdentityUser user, Claim claim)
        {
            var uc = new ApplicationUserClaimDto();
            uc.ClaimType = claim.Type;
            uc.ClaimValue = claim.Value;
            uc.UserStorageKey = user.Id.ToString();
            var resp = _applicationUserClaimApiService.Create(uc);
            if (resp.Success)
                return _mapper.Map<ApplicationIdentityUserClaim>(uc);
            return null;
        }

        public override async Task<System.Collections.Generic.IList<Claim>> GetClaimsAsync(ApplicationIdentityUser user, CancellationToken cancellationToken = default)
        {
            var userDto = _mapper.Map<ApplicationUserDto>(user);
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationUserClaimDto.UserStorageKey), userDto.StorageKey);
            var respUserClaims = await _applicationUserClaimApiService.QueryAsync(queryBuilder.Build());
            if (respUserClaims.Success && respUserClaims.Item.List.Count > 0)
            {
                var userClaims = _mapper.Map<List<ApplicationIdentityUserClaim>>(respUserClaims.Item.List);
                return userClaims.Select(x => x.ToClaim()).ToList();
            }
            return new List<Claim>();
        }

        public override async Task AddToRoleAsync(ApplicationIdentityUser user, string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationRoleDto.NormalizedName), normalizedRoleName);
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

        public override async Task<ApplicationIdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            var respUser = await _applicationUserApiService.GetAsync(userId);
            if (respUser.Item != null)
                return _mapper.Map<ApplicationIdentityUser>(respUser.Item);
            return null;
        }

        public override async Task<ApplicationIdentityUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationUserDto.NormalizedEmail), normalizedEmail.ToUpper());
            var respQuery = await _applicationUserApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
                return _mapper.Map<ApplicationIdentityUser>(respQuery.Item.List[0]);
            return null;
        }

        public override async Task RemoveClaimsAsync(ApplicationIdentityUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            if (claims != null && claims.Count() > 0)
            {
                var userDto = _mapper.Map<ApplicationUserDto>(user);
                ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
                queryBuilder.IsEqual(nameof(ApplicationUserClaimDto.UserStorageKey), userDto.StorageKey);
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

        public override async Task<System.Collections.Generic.IList<ApplicationIdentityUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationUserClaimDto.ClaimType), claim.Type);
            var respUserClaims = await _applicationUserClaimApiService.QueryAsync(queryBuilder.Build());

            queryBuilder = new ServiceQueryRequestBuilder();
            var respUsers = await _applicationUserApiService.QueryAsync(queryBuilder.Build());
            var users = _mapper.Map<List<ApplicationIdentityUser>>(respUsers.Item.List);
            return users;
        }

        public override async Task ReplaceClaimAsync(ApplicationIdentityUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default)
        {
            var userDto = _mapper.Map<ApplicationUserDto>(user);
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationUserClaimDto.UserStorageKey), userDto.StorageKey);
            queryBuilder.And();
            queryBuilder.IsEqual(nameof(ApplicationUserClaimDto.ClaimType), claim.Type);
            var respQuery = await _applicationUserClaimApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
            {
                var claimDto = respQuery.Item.List[0];
                claimDto.ClaimType = newClaim.Type;
                claimDto.ClaimValue = newClaim.Value;
                await _applicationUserClaimApiService.UpdateAsync(claimDto);
            }
        }

        protected override ApplicationUserRole CreateUserRole(ApplicationIdentityUser user, ApplicationIdentityRole role)
        {
            var item = new ApplicationUserRoleDto()
            {
                UserStorageKey = user.Id.ToString(),
                RoleStorageKey = role.Id.ToString()
            };
            var resp = _applicationUserRoleApiService.Create(item);
            return _mapper.Map<ApplicationUserRole>(item);
        }

        protected override async Task<ApplicationIdentityRole> FindRoleAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationRoleDto.NormalizedName), normalizedRoleName);
            var respQuery = await _applicationRoleApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.Count > 0)
                return _mapper.Map<ApplicationIdentityRole>(respQuery.Item.List[0]);
            return null;
        }

        protected override async Task<ApplicationUserRole> FindUserRoleAsync(string userId, string roleId, CancellationToken cancellationToken)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationUserRoleDto.UserStorageKey), userId.ToString() + StorageMongoDbConstants.STORAGEKEY_DELIMITER);
            queryBuilder.And();
            queryBuilder.IsEqual(nameof(ApplicationUserRoleDto.RoleStorageKey), roleId.ToString() + StorageMongoDbConstants.STORAGEKEY_DELIMITER);
            var respQuery = await _applicationUserRoleApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.Count > 0)
                return _mapper.Map<ApplicationUserRole>(respQuery.Item.List[0]);
            return null;
        }

        public override async Task<System.Collections.Generic.IList<string>> GetRolesAsync(ApplicationIdentityUser user, CancellationToken cancellationToken = default)
        {
            var userDto = _mapper.Map<ApplicationUserDto>(user);
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationUserRoleDto.UserStorageKey), userDto.StorageKey);
            var respQuery = await _applicationUserRoleApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
            {
                var roleIds = respQuery.Item.List.Select(x => x.RoleStorageKey.ToString()).ToArray();
                queryBuilder = new ServiceQueryRequestBuilder();
                queryBuilder.IsInSet(nameof(ApplicationRoleDto.StorageKey), roleIds);
                var respRoles = await _applicationRoleApiService.QueryAsync(queryBuilder.Build());
                if (respRoles.Success && respRoles.Item.List.Count > 0)
                    return respRoles.Item.List.Select(x => x.Name).ToList();
            }
            return new List<string>();
        }

        public override async Task<System.Collections.Generic.IList<ApplicationIdentityUser>> GetUsersInRoleAsync(string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationRoleDto.NormalizedName), normalizedRoleName);
            var respRoles = await _applicationRoleApiService.QueryAsync(queryBuilder.Build());
            if (respRoles.Success && respRoles.Item.List.Count > 0)
            {
                var role = respRoles.Item.List[0];
                queryBuilder = new ServiceQueryRequestBuilder();
                queryBuilder.IsEqual(nameof(ApplicationUserRoleDto.RoleStorageKey), role.StorageKey);
                var respUserRoles = await _applicationUserRoleApiService.QueryAsync(queryBuilder.Build());
                if (respUserRoles.Success && respUserRoles.Item.List.Count > 0)
                {
                    var userIds = respUserRoles.Item.List.Select(x => x.UserStorageKey.ToString()).ToArray();
                    queryBuilder = new ServiceQueryRequestBuilder();
                    queryBuilder.IsInSet(nameof(ApplicationUserDto.StorageKey), userIds);
                    var respUsers = await _applicationUserApiService.QueryAsync(queryBuilder.Build());
                    if (respUsers.Success && respUsers.Item.List.Count > 0)
                        return _mapper.Map<List<ApplicationIdentityUser>>(respUsers.Item.List);
                }
            }
            return new List<ApplicationIdentityUser>();
        }

        public override async Task<bool> IsInRoleAsync(ApplicationIdentityUser user, string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationRoleDto.NormalizedName), normalizedRoleName);
            var respRoles = await _applicationRoleApiService.QueryAsync(queryBuilder.Build());
            if (respRoles.Success && respRoles.Item.List.Count > 0)
            {
                var userDto = _mapper.Map<ApplicationUserDto>(user);
                var role = respRoles.Item.List[0];
                queryBuilder = new ServiceQueryRequestBuilder();
                queryBuilder.IsEqual(nameof(ApplicationUserRoleDto.RoleStorageKey), role.StorageKey);
                queryBuilder.And();
                queryBuilder.IsEqual(nameof(ApplicationUserRoleDto.UserStorageKey), userDto.StorageKey);
                var respUserRoles = await _applicationUserRoleApiService.QueryAsync(queryBuilder.Build());
                if (respUserRoles.Success && respUserRoles.Item.List.Count > 0)
                    return true;
            }
            return false;
        }

        public override async Task RemoveFromRoleAsync(ApplicationIdentityUser user, string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationRoleDto.NormalizedName), normalizedRoleName);
            var respRoles = await _applicationRoleApiService.QueryAsync(queryBuilder.Build());
            if (respRoles.Success && respRoles.Item.List.Count > 0)
            {
                var userDto = _mapper.Map<ApplicationUserDto>(user);
                var role = respRoles.Item.List[0];
                queryBuilder = new ServiceQueryRequestBuilder();
                queryBuilder.IsEqual(nameof(ApplicationUserRoleDto.RoleStorageKey), role.StorageKey);
                queryBuilder.And();
                queryBuilder.IsEqual(nameof(ApplicationUserRoleDto.UserStorageKey), userDto.StorageKey);
                var respUserRoles = await _applicationUserRoleApiService.QueryAsync(queryBuilder.Build());
                if (respUserRoles.Success && respUserRoles.Item.List.Count > 0)
                    await _applicationUserRoleApiService.DeleteAsync(respUserRoles.Item.List[0].StorageKey);
            }
        }

        public override async Task<ApplicationIdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationUserDto.NormalizedUserName), normalizedUserName);
            var respQuery = await _applicationUserApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
                return _mapper.Map<ApplicationIdentityUser>(respQuery.Item.List[0]);
            return null;
        }

        protected override async Task<ApplicationIdentityUser> FindUserAsync(string userId, CancellationToken cancellationToken)
        {
            var respUser = await _applicationUserApiService.GetAsync(userId.ToString());
            return _mapper.Map<ApplicationIdentityUser>(respUser.Item);
        }

        protected override ApplicationUserLogin CreateUserLogin(ApplicationIdentityUser user, UserLoginInfo login)
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

        protected override ApplicationUserToken CreateUserToken(ApplicationIdentityUser user, string loginProvider, string name, string value)
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

        protected override async Task<ApplicationUserLogin> FindUserLoginAsync(string userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder
                .IsEqual(nameof(ApplicationUserLoginDto.UserStorageKey), userId.ToString())
                .And()
                .IsEqual(nameof(ApplicationUserLoginDto.LoginProvider), loginProvider)
                .And()
                .IsEqual(nameof(ApplicationUserLoginDto.ProviderKey), providerKey);
            var respQuery = await _applicationUserLoginApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
                return _mapper.Map<ApplicationUserLogin>(respQuery.Item.List[0]);
            return null;
        }

        protected override async Task<ApplicationUserLogin> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder
                .IsEqual(nameof(ApplicationUserLoginDto.LoginProvider), loginProvider)
                .And()
                .IsEqual(nameof(ApplicationUserLoginDto.ProviderKey), providerKey);
            var respQuery = await _applicationUserLoginApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
                return _mapper.Map<ApplicationUserLogin>(respQuery.Item.List[0]);
            return null;
        }

        public override async Task AddLoginAsync(ApplicationIdentityUser user, UserLoginInfo login, CancellationToken cancellationToken = default)
        {
            ApplicationUserLoginDto obj = new ApplicationUserLoginDto();
            obj.LoginProvider = login.LoginProvider;
            obj.ProviderDisplayName = login.ProviderDisplayName;
            obj.ProviderKey = login.ProviderKey;
            obj.UserStorageKey = user.Id.ToString();
            await _applicationUserLoginApiService.CreateAsync(obj);
        }

        public override async Task RemoveLoginAsync(ApplicationIdentityUser user, string loginProvider, string providerKey, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder
                .IsEqual(nameof(ApplicationUserLoginDto.UserStorageKey), user.Id.ToString())
                .And()
                .IsEqual(nameof(ApplicationUserLoginDto.LoginProvider), loginProvider)
                .And()
                .IsEqual(nameof(ApplicationUserLoginDto.ProviderKey), providerKey);
            var respQuery = await _applicationUserLoginApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
                await _applicationUserLoginApiService.DeleteAsync(respQuery.Item.List[0].StorageKey);
        }

        public override async Task<System.Collections.Generic.IList<UserLoginInfo>> GetLoginsAsync(ApplicationIdentityUser user, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder
                .IsEqual(nameof(ApplicationUserLoginDto.UserStorageKey), user.Id.ToString());
            var respQuery = await _applicationUserLoginApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
            {
                var list = new List<UserLoginInfo>();
                foreach (var item in respQuery.Item.List)
                    list.Add(new UserLoginInfo(item.LoginProvider, item.ProviderKey, item.ProviderDisplayName));
                return list;
            }
            return new List<UserLoginInfo>();
        }

        protected override async Task<ApplicationUserToken> FindTokenAsync(ApplicationIdentityUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder
                .IsEqual(nameof(ApplicationUserTokenDto.UserStorageKey), user.Id.ToString())
                .And()
                .IsEqual(nameof(ApplicationUserTokenDto.LoginProvider), loginProvider)
                .And()
                .IsEqual(nameof(ApplicationUserTokenDto.Name), name);
            var respQuery = await _applicationUserTokenApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
                return _mapper.Map<ApplicationUserToken>(respQuery.Item.List[0]);
            return null;
        }

        protected override async Task AddUserTokenAsync(ApplicationUserToken token)
        {
            var dto = _mapper.Map<ApplicationUserTokenDto>(token);
            await _applicationUserTokenApiService.CreateAsync(dto);
        }

        protected override async Task RemoveUserTokenAsync(ApplicationUserToken token)
        {
            var dto = _mapper.Map<ApplicationUserTokenDto>(token);
            await _applicationUserTokenApiService.DeleteAsync(dto.StorageKey);
        }

        public override IQueryable<ApplicationIdentityUser> Users
        {
            get
            {
                var respUsers = _applicationUserApiService.Query(new ServiceQueryRequest());
                var users = _mapper.Map<List<ApplicationIdentityUser>>(respUsers.Item.List);
                return users.AsQueryable();
            }
        }
    }
}