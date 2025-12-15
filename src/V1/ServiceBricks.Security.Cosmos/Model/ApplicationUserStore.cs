using Microsoft.AspNetCore.Identity;
using ServiceQuery;
using System.Collections.Generic;
using System.Security.Claims;

namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// This is user storage.
    /// </summary>
    public partial class ApplicationUserStore : UserStoreBase<ApplicationUser, ApplicationRole, Guid, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationUserToken, ApplicationRoleClaim>
    {
        protected readonly IMapper _mapper;
        protected readonly IBusinessRuleService _businessRuleService;
        protected readonly IUserApiService _applicationUserApiService;
        protected readonly IUserRoleApiService _applicationUserRoleApiService;
        protected readonly IUserClaimApiService _applicationUserClaimApiService;
        protected readonly IUserLoginApiService _applicationUserLoginApiService;
        protected readonly IUserTokenApiService _applicationUserTokenApiService;
        protected readonly IRoleApiService _applicationRoleApiService;
        protected readonly SecurityCosmosContext _context;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="businessRuleService"></param>
        /// <param name="applicationUserApiService"></param>
        /// <param name="applicationUserRoleApiService"></param>
        /// <param name="applicationUserClaimApiService"></param>
        /// <param name="applicationUserLoginApiService"></param>
        /// <param name="applicationUserTokenApiService"></param>
        /// <param name="applicationRoleApiService"></param>
        /// <param name="context"></param>
        /// <param name="describer"></param>
        public ApplicationUserStore(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IUserApiService applicationUserApiService,
            IUserRoleApiService applicationUserRoleApiService,
            IUserClaimApiService applicationUserClaimApiService,
            IUserLoginApiService applicationUserLoginApiService,
            IUserTokenApiService applicationUserTokenApiService,
            IRoleApiService applicationRoleApiService,
            SecurityCosmosContext context,
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
            _context = context;
        }

        /// <summary>
        /// Create a user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (user.Id == Guid.Empty)
                user.Id = Guid.NewGuid();
            var userDto = _mapper.Map<ApplicationUser, UserDto>(user);
            var resp = await _applicationUserApiService.CreateAsync(userDto);
            if (resp.Success)
                _mapper.Map<UserDto, ApplicationUser>(resp.Item, user);
            return resp.GetIdentityResult();
        }

        /// <summary>
        /// Update a user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            var userDto = _mapper.Map<ApplicationUser, UserDto>(user);
            var resp = await _applicationUserApiService.UpdateAsync(userDto);
            if (resp.Success)
                _mapper.Map<UserDto, ApplicationUser>(resp.Item, user);
            return resp.GetIdentityResult();
        }

        /// <summary>
        /// Delete a user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            var userDto = _mapper.Map<ApplicationUser, UserDto>(user);
            var resp = await _applicationUserApiService.DeleteAsync(userDto.StorageKey);
            return resp.GetIdentityResult();
        }

        /// <summary>
        /// Add a claim to a user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="claims"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task AddClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            foreach (var claim in claims)
            {
                var uc = new UserClaimDto();
                uc.ClaimType = claim.Type;
                uc.ClaimValue = claim.Value;
                uc.UserStorageKey = user.Id.ToString();
                _ = await _applicationUserClaimApiService.CreateAsync(uc);
            }
        }

        /// <summary>
        /// Remove a claim from a user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="claim"></param>
        /// <returns></returns>
        protected override ApplicationUserClaim CreateUserClaim(ApplicationUser user, Claim claim)
        {
            var uc = new UserClaimDto();
            uc.ClaimType = claim.Type;
            uc.ClaimValue = claim.Value;
            uc.UserStorageKey = user.Id.ToString();
            var resp = _applicationUserClaimApiService.Create(uc);
            if (resp.Success)
                return _mapper.Map<UserClaimDto, ApplicationUserClaim>(uc);
            return null;
        }

        /// <summary>
        /// Get claims for a user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<System.Collections.Generic.IList<Claim>> GetClaimsAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            var userDto = _mapper.Map<ApplicationUser, UserDto>(user);
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(UserClaimDto.UserStorageKey), userDto.StorageKey);
            var respUserClaims = await _applicationUserClaimApiService.QueryAsync(queryBuilder.Build());
            if (respUserClaims.Success && respUserClaims.Item.List.Count > 0)
            {
                var userClaims = _mapper.Map< List < UserClaimDto> , List <ApplicationUserClaim>>(respUserClaims.Item.List);
                return userClaims.Select(x => x.ToClaim()).ToList();
            }
            return new List<Claim>();
        }

        /// <summary>
        /// Add a user to a role.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="normalizedRoleName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task AddToRoleAsync(ApplicationUser user, string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(RoleDto.NormalizedName), normalizedRoleName);
            var respRole = await _applicationRoleApiService.QueryAsync(queryBuilder.Build());
            if (respRole.Success && respRole.Item.List.Count > 0)
            {
                var role = respRole.Item.List[0];
                var userRole = new UserRoleDto()
                {
                    UserStorageKey = user.Id.ToString(),
                    RoleStorageKey = role.StorageKey
                };
                await _applicationUserRoleApiService.CreateAsync(userRole);
            }
        }

        /// <summary>
        /// Find a user by id.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            var respUser = await _applicationUserApiService.GetAsync(userId);
            if (respUser.Item != null)
                return _mapper.Map<UserDto, ApplicationUser>(respUser.Item);
            return null;
        }

        /// <summary>
        /// Find a user by email.
        /// </summary>
        /// <param name="normalizedEmail"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<ApplicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(UserDto.NormalizedEmail), normalizedEmail);
            var respQuery = await _applicationUserApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
                return _mapper.Map<UserDto, ApplicationUser>(respQuery.Item.List[0]);
            return null;
        }

        /// <summary>
        /// Remove claims from a user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="claims"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task RemoveClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            if (claims != null && claims.Count() > 0)
            {
                var userDto = _mapper.Map<ApplicationUser, UserDto>(user);
                ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
                queryBuilder.IsEqual(nameof(UserClaimDto.UserStorageKey), userDto.StorageKey);
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

        /// <summary>
        /// Get users for a claim.
        /// </summary>
        /// <param name="claim"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<System.Collections.Generic.IList<ApplicationUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(UserClaimDto.ClaimType), claim.Type);
            queryBuilder.Select(nameof(UserClaimDto.UserStorageKey));
            var respUserClaims = await _applicationUserClaimApiService.QueryAsync(queryBuilder.Build());

            queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsInSet(nameof(UserDto.StorageKey), respUserClaims.Item.List.Select(x => x.UserStorageKey).ToArray());
            var respUsers = await _applicationUserApiService.QueryAsync(queryBuilder.Build());
            var users = _mapper.Map< List < UserDto > , List <ApplicationUser>>(respUsers.Item.List);
            return users;
        }

        /// <summary>
        /// Replace a claim.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="claim"></param>
        /// <param name="newClaim"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task ReplaceClaimAsync(ApplicationUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default)
        {
            var userDto = _mapper.Map<ApplicationUser, UserDto>(user);
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(UserClaimDto.UserStorageKey), userDto.StorageKey);
            queryBuilder.And();
            queryBuilder.IsEqual(nameof(UserClaimDto.ClaimType), claim.Type);
            var respQuery = await _applicationUserClaimApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
            {
                var claimDto = respQuery.Item.List[0];
                claimDto.ClaimType = newClaim.Type;
                claimDto.ClaimValue = newClaim.Value;
                await _applicationUserClaimApiService.UpdateAsync(claimDto);
            }
        }

        /// <summary>
        /// Create a user role.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        protected override ApplicationUserRole CreateUserRole(ApplicationUser user, ApplicationRole role)
        {
            var item = new UserRoleDto()
            {
                UserStorageKey = user.Id.ToString(),
                RoleStorageKey = role.Id.ToString()
            };
            var resp = _applicationUserRoleApiService.Create(item);
            return _mapper.Map<UserRoleDto, ApplicationUserRole>(item);
        }

        /// <summary>
        /// Find role
        /// </summary>
        /// <param name="normalizedRoleName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<ApplicationRole> FindRoleAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(RoleDto.NormalizedName), normalizedRoleName);
            var respQuery = await _applicationRoleApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
                return _mapper.Map<RoleDto, ApplicationRole>(respQuery.Item.List[0]);
            return null;
        }

        /// <summary>
        /// Find a user role.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<ApplicationUserRole> FindUserRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(UserRoleDto.UserStorageKey), userId.ToString());
            queryBuilder.And();
            queryBuilder.IsEqual(nameof(UserRoleDto.RoleStorageKey), roleId.ToString());
            var respQuery = await _applicationUserRoleApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
                return _mapper.Map<UserRoleDto, ApplicationUserRole>(respQuery.Item.List[0]);
            return null;
        }

        /// <summary>
        /// Get roles for a user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<System.Collections.Generic.IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            var userDto = _mapper.Map<ApplicationUser, UserDto>(user);
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(UserRoleDto.UserStorageKey), userDto.StorageKey);
            var respQuery = await _applicationUserRoleApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
            {
                var roleIds = respQuery.Item.List.Select(x => x.RoleStorageKey.ToString()).ToArray();
                queryBuilder = new ServiceQueryRequestBuilder();
                queryBuilder.IsInSet(nameof(RoleDto.StorageKey), roleIds);
                var respRoles = await _applicationRoleApiService.QueryAsync(queryBuilder.Build());
                if (respRoles.Success && respRoles.Item.List.Count > 0)
                    return respRoles.Item.List.Select(x => x.Name).ToList();
            }
            return new List<string>();
        }

        /// <summary>
        /// Get users in a role.
        /// </summary>
        /// <param name="normalizedRoleName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<System.Collections.Generic.IList<ApplicationUser>> GetUsersInRoleAsync(string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(RoleDto.NormalizedName), normalizedRoleName);
            var respRoles = await _applicationRoleApiService.QueryAsync(queryBuilder.Build());
            if (respRoles.Success && respRoles.Item.List.Count > 0)
            {
                var role = respRoles.Item.List[0];
                queryBuilder = new ServiceQueryRequestBuilder();
                queryBuilder.IsEqual(nameof(UserRoleDto.RoleStorageKey), role.StorageKey);
                var respUserRoles = await _applicationUserRoleApiService.QueryAsync(queryBuilder.Build());
                if (respUserRoles.Success && respUserRoles.Item.List.Count > 0)
                {
                    var userIds = respUserRoles.Item.List.Select(x => x.UserStorageKey.ToString()).ToArray();
                    queryBuilder = new ServiceQueryRequestBuilder();
                    queryBuilder.IsInSet(nameof(UserDto.StorageKey), userIds);
                    var respUsers = await _applicationUserApiService.QueryAsync(queryBuilder.Build());
                    if (respUsers.Success && respUsers.Item.List.Count > 0)
                        return _mapper.Map< List < UserDto > , List <ApplicationUser>>(respUsers.Item.List);
                }
            }
            return new List<ApplicationUser>();
        }

        /// <summary>
        /// Determine if a user is in a role.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="normalizedRoleName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<bool> IsInRoleAsync(ApplicationUser user, string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(RoleDto.NormalizedName), normalizedRoleName);
            var respRoles = await _applicationRoleApiService.QueryAsync(queryBuilder.Build());
            if (respRoles.Success && respRoles.Item.List.Count > 0)
            {
                var userDto = _mapper.Map<ApplicationUser, UserDto>(user);
                var role = respRoles.Item.List[0];
                queryBuilder = new ServiceQueryRequestBuilder();
                queryBuilder.IsEqual(nameof(UserRoleDto.RoleStorageKey), role.StorageKey);
                queryBuilder.And();
                queryBuilder.IsEqual(nameof(UserRoleDto.UserStorageKey), userDto.StorageKey);
                var respUserRoles = await _applicationUserRoleApiService.QueryAsync(queryBuilder.Build());
                if (respUserRoles.Success && respUserRoles.Item.List.Count > 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Remove a user from a role.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="normalizedRoleName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task RemoveFromRoleAsync(ApplicationUser user, string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(RoleDto.NormalizedName), normalizedRoleName);
            var respRoles = await _applicationRoleApiService.QueryAsync(queryBuilder.Build());
            if (respRoles.Success && respRoles.Item.List.Count > 0)
            {
                var userDto = _mapper.Map<ApplicationUser, UserDto>(user);
                var role = respRoles.Item.List[0];
                queryBuilder = new ServiceQueryRequestBuilder();
                queryBuilder.IsEqual(nameof(UserRoleDto.RoleStorageKey), role.StorageKey);
                queryBuilder.And();
                queryBuilder.IsEqual(nameof(UserRoleDto.UserStorageKey), userDto.StorageKey);
                var respUserRoles = await _applicationUserRoleApiService.QueryAsync(queryBuilder.Build());
                if (respUserRoles.Success && respUserRoles.Item.List.Count > 0)
                    await _applicationUserRoleApiService.DeleteAsync(respUserRoles.Item.List[0].StorageKey);
            }
        }

        /// <summary>
        /// Find a user by name.
        /// </summary>
        /// <param name="normalizedUserName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(UserDto.NormalizedUserName), normalizedUserName);
            var respQuery = await _applicationUserApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
                return _mapper.Map<UserDto, ApplicationUser>(respQuery.Item.List[0]);
            return null;
        }

        /// <summary>
        /// Find a user by id.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<ApplicationUser> FindUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            var respUser = await _applicationUserApiService.GetAsync(userId.ToString());
            return _mapper.Map<UserDto, ApplicationUser>(respUser.Item);
        }

        /// <summary>
        /// Create a user login
        /// </summary>
        /// <param name="user"></param>
        /// <param name="login"></param>
        /// <returns></returns>
        protected override ApplicationUserLogin CreateUserLogin(ApplicationUser user, UserLoginInfo login)
        {
            var ul = new UserLoginDto()
            {
                LoginProvider = login.LoginProvider,
                ProviderDisplayName = login.ProviderDisplayName,
                ProviderKey = login.ProviderKey,
                UserStorageKey = user.Id.ToString(),
            };
            var resp = _applicationUserLoginApiService.Create(ul);
            if (resp.Success)
                return _mapper.Map<UserLoginDto, ApplicationUserLogin>(resp.Item);
            return null;
        }

        /// <summary>
        /// Create a user token
        /// </summary>
        /// <param name="user"></param>
        /// <param name="loginProvider"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override ApplicationUserToken CreateUserToken(ApplicationUser user, string loginProvider, string name, string value)
        {
            var ut = new UserTokenDto()
            {
                LoginProvider = loginProvider,
                Name = name,
                UserStorageKey = user.Id.ToString(),
                Value = value,
            };
            var resp = _applicationUserTokenApiService.Create(ut);
            if (resp.Success)
                return _mapper.Map<UserTokenDto, ApplicationUserToken>(ut);
            return null;
        }

        /// <summary>
        /// Find a user login
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<ApplicationUserLogin> FindUserLoginAsync(Guid userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder
                .IsEqual(nameof(UserLoginDto.UserStorageKey), userId.ToString())
                .And()
                .IsEqual(nameof(UserLoginDto.LoginProvider), loginProvider)
                .And()
                .IsEqual(nameof(UserLoginDto.ProviderKey), providerKey);
            var respQuery = await _applicationUserLoginApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
                return _mapper.Map<UserLoginDto, ApplicationUserLogin>(respQuery.Item.List[0]);
            return null;
        }

        /// <summary>
        /// Find a user login
        /// </summary>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<ApplicationUserLogin> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder
                .IsEqual(nameof(UserLoginDto.LoginProvider), loginProvider)
                .And()
                .IsEqual(nameof(UserLoginDto.ProviderKey), providerKey);
            var respQuery = await _applicationUserLoginApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
                return _mapper.Map<UserLoginDto, ApplicationUserLogin>(respQuery.Item.List[0]);
            return null;
        }

        /// <summary>
        /// Add a login to a user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="login"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task AddLoginAsync(ApplicationUser user, UserLoginInfo login, CancellationToken cancellationToken = default)
        {
            UserLoginDto obj = new UserLoginDto();
            obj.LoginProvider = login.LoginProvider;
            obj.ProviderDisplayName = login.ProviderDisplayName;
            obj.ProviderKey = login.ProviderKey;
            obj.UserStorageKey = user.Id.ToString();
            await _applicationUserLoginApiService.CreateAsync(obj);
        }

        /// <summary>
        /// Remove a login from a user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task RemoveLoginAsync(ApplicationUser user, string loginProvider, string providerKey, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder
                .IsEqual(nameof(UserLoginDto.UserStorageKey), user.Id.ToString())
                .And()
                .IsEqual(nameof(UserLoginDto.LoginProvider), loginProvider)
                .And()
                .IsEqual(nameof(UserLoginDto.ProviderKey), providerKey);
            var respQuery = await _applicationUserLoginApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
                await _applicationUserLoginApiService.DeleteAsync(respQuery.Item.List[0].StorageKey);
        }

        /// <summary>
        /// Get logins for a user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<System.Collections.Generic.IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder
                .IsEqual(nameof(UserLoginDto.UserStorageKey), user.Id.ToString());
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

        /// <summary>
        /// Find a user token.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="loginProvider"></param>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<ApplicationUserToken> FindTokenAsync(ApplicationUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder
                .IsEqual(nameof(UserTokenDto.UserStorageKey), user.Id.ToString())
                .And()
                .IsEqual(nameof(UserTokenDto.LoginProvider), loginProvider)
                .And()
                .IsEqual(nameof(UserTokenDto.Name), name);
            var respQuery = await _applicationUserTokenApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
                return _mapper.Map<UserTokenDto, ApplicationUserToken>(respQuery.Item.List[0]);
            return null;
        }

        /// <summary>
        /// Add user token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected override async Task AddUserTokenAsync(ApplicationUserToken token)
        {
            var dto = _mapper.Map<ApplicationUserToken, UserTokenDto>(token);
            await _applicationUserTokenApiService.CreateAsync(dto);
        }

        /// <summary>
        /// Remove user token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected override async Task RemoveUserTokenAsync(ApplicationUserToken token)
        {
            var dto = _mapper.Map<ApplicationUserToken, UserTokenDto>(token);
            await _applicationUserTokenApiService.DeleteAsync(dto.StorageKey);
        }

        /// <summary>
        /// Query users
        /// </summary>
        public override IQueryable<ApplicationUser> Users
        {
            get
            {
                return _context.Users.AsQueryable();
            }
        }
    }
}