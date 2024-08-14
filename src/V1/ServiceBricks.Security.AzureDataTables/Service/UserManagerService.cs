using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ServiceQuery;
using System.Security.Claims;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is a API service for the ApplicationUser domain object.
    /// </summary>
    public partial class UserManagerService : IUserManagerService
    {
        protected readonly IMapper _mapper;
        protected readonly IApplicationUserApiService _applicationUserApiService;
        protected readonly IApplicationUserClaimApiService _applicationUserClaimApiService;
        protected readonly IApplicationUserLoginApiService _applicationUserLoginApiService;
        protected readonly IApplicationUserRoleApiService _applicationUserRoleApiService;
        protected readonly IApplicationUserTokenApiService _applicationUserTokenApiService;
        protected readonly IApplicationRoleApiService _applicationRoleApiService;
        protected readonly IApplicationRoleClaimApiService _applicationRoleClaimApiService;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly SignInManager<ApplicationUser> _signInManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="applicationUserApiService"></param>
        /// <param name="applicationUserClaimApiService"></param>
        /// <param name="applicationUserLoginApiService"></param>
        /// <param name="applicationUserRoleApiService"></param>
        /// <param name="applicationUserTokenApiService"></param>
        /// <param name="applicationRoleApiService"></param>
        /// <param name="applicationRoleClaimApiService"></param>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        public UserManagerService(
            IMapper mapper,
            IApplicationUserApiService applicationUserApiService,
            IApplicationUserClaimApiService applicationUserClaimApiService,
            IApplicationUserLoginApiService applicationUserLoginApiService,
            IApplicationUserRoleApiService applicationUserRoleApiService,
            IApplicationUserTokenApiService applicationUserTokenApiService,
            IApplicationRoleApiService applicationRoleApiService,
            IApplicationRoleClaimApiService applicationRoleClaimApiService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _mapper = mapper;
            _applicationUserApiService = applicationUserApiService;
            _applicationUserClaimApiService = applicationUserClaimApiService;
            _applicationUserLoginApiService = applicationUserLoginApiService;
            _applicationUserRoleApiService = applicationUserRoleApiService;
            _applicationUserTokenApiService = applicationUserTokenApiService;
            _applicationRoleApiService = applicationRoleApiService;
            _applicationRoleClaimApiService = applicationRoleClaimApiService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Find a user by email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public virtual IResponseItem<ApplicationUserDto> FindByEmail(string email)
        {
            var response = new ResponseItem<ApplicationUserDto>();
            if (string.IsNullOrEmpty(email))
                return response;

            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationUser.NormalizedEmail), email.ToUpper());
            var respQuery = _applicationUserApiService.Query(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
                response.Item = respQuery.Item.List[0];
            else
                response.CopyFrom(respQuery);
            return response;
        }

        /// <summary>
        /// Find a user by email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public virtual async Task<IResponseItem<ApplicationUserDto>> FindByEmailAsync(string email)
        {
            var response = new ResponseItem<ApplicationUserDto>();
            if (string.IsNullOrEmpty(email))
                return response;

            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationUser.NormalizedEmail), email.ToUpper());
            var respQuery = await _applicationUserApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
                response.Item = respQuery.Item.List[0];
            else
                response.CopyFrom(respQuery);
            return response;
        }

        /// <summary>
        /// Find a user by user storage key.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <returns></returns>
        public virtual IResponseItem<ApplicationUserDto> FindById(string userStorageKey)
        {
            var response = new ResponseItem<ApplicationUserDto>();
            if (string.IsNullOrEmpty(userStorageKey))
                return response;

            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationUser.Id), userStorageKey);
            var respQuery = _applicationUserApiService.Query(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
                response.Item = respQuery.Item.List[0];
            else
                response.CopyFrom(respQuery);
            return response;
        }

        /// <summary>
        /// Find a user by user storage key.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <returns></returns>
        public virtual async Task<IResponseItem<ApplicationUserDto>> FindByIdAsync(string userStorageKey)
        {
            var response = new ResponseItem<ApplicationUserDto>();
            if (string.IsNullOrEmpty(userStorageKey))
                return response;

            ServiceQueryRequestBuilder queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(ApplicationUser.Id), userStorageKey);
            var respQuery = await _applicationUserApiService.QueryAsync(queryBuilder.Build());
            if (respQuery.Success && respQuery.Item.List.Count > 0)
                response.Item = respQuery.Item.List[0];
            else
                response.CopyFrom(respQuery);
            return response;
        }

        /// <summary>
        /// Confirm email.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public virtual IResponse ConfirmEmail(string userStorageKey, string code)
        {
            return ConfirmEmailAsync(userStorageKey, code).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Confirm email.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public virtual async Task<IResponse> ConfirmEmailAsync(string userStorageKey, string code)
        {
            var response = new Response();
            var respUser = await _userManager.FindByIdAsync(userStorageKey);
            if (respUser == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                return response;
            }
            var result = await _userManager.ConfirmEmailAsync(respUser, code);
            response.CopyFrom(result);
            return response;
        }

        /// <summary>
        /// Generate a password reset token.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <returns></returns>
        public IResponseItem<string> GeneratePasswordResetToken(string userStorageKey)
        {
            return GeneratePasswordResetTokenAsync(userStorageKey).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Generate a password reset token.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <returns></returns>
        public async Task<IResponseItem<string>> GeneratePasswordResetTokenAsync(string userStorageKey)
        {
            var response = new ResponseItem<string>();
            if (string.IsNullOrEmpty(userStorageKey))
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                return response;
            }

            var user = await _userManager.FindByIdAsync(userStorageKey);
            if (user == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                return response;
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (string.IsNullOrEmpty(code))
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_SYSTEM));
                return response;
            }
            response.Item = code;
            return response;
        }

        /// <summary>
        /// Reset password.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="code"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public virtual IResponse ResetPassword(string userStorageKey, string code, string password)
        {
            return ResetPasswordAsync(userStorageKey, code, password).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Reset password.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="code"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public virtual async Task<IResponse> ResetPasswordAsync(string userStorageKey, string code, string password)
        {
            var response = new Response();
            if (string.IsNullOrEmpty(userStorageKey))
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                return response;
            }

            var user = await _userManager.FindByIdAsync(userStorageKey);
            if (user == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                return response;
            }

            var result = await _userManager.ResetPasswordAsync(user, code, password);
            response.CopyFrom(result);
            return response;
        }

        /// <summary>
        /// Change password.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="currentPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public IResponse ChangePassword(string userStorageKey, string currentPassword, string newPassword)
        {
            return ChangePasswordAsync(userStorageKey, currentPassword, newPassword).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Change password.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="currentPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public async Task<IResponse> ChangePasswordAsync(string userStorageKey, string currentPassword, string newPassword)
        {
            var response = new Response();
            var user = await _userManager.FindByIdAsync(userStorageKey.ToString());
            if (user == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                return response;
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            response.CopyFrom(result);
            return response;
        }

        /// <summary>
        /// Refresh sign in.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <returns></returns>
        public IResponse RefreshSignIn(string userStorageKey)
        {
            return RefreshSignInAsync(userStorageKey).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Refresh sign in.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <returns></returns>
        public async Task<IResponse> RefreshSignInAsync(string userStorageKey)
        {
            var response = new Response();
            var user = await _userManager.FindByIdAsync(userStorageKey);
            if (user == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                return response;
            }
            await _signInManager.RefreshSignInAsync(user);
            return response;
        }

        /// <summary>
        /// Get 2FA user.
        /// </summary>
        /// <returns></returns>
        public IResponseItem<ApplicationUserDto> GetTwoFactorAuthenticationUser()
        {
            return GetTwoFactorAuthenticationUserAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get 2FA user.
        /// </summary>
        /// <returns></returns>
        public async Task<IResponseItem<ApplicationUserDto>> GetTwoFactorAuthenticationUserAsync()
        {
            var response = new ResponseItem<ApplicationUserDto>();
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                return response;
            }
            response.Item = _mapper.Map<ApplicationUserDto>(user);
            return response;
        }

        /// <summary>
        /// Generate 2FA token.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public IResponseItem<string> GenerateTwoFactorToken(string userStorageKey, string provider)
        {
            return GenerateTwoFactorTokenAsync(userStorageKey, provider).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Generate 2FA token.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public async Task<IResponseItem<string>> GenerateTwoFactorTokenAsync(string userStorageKey, string provider)
        {
            var response = new ResponseItem<string>();
            var user = await _userManager.FindByIdAsync(userStorageKey);
            if (user == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                return response;
            }
            var code = await _userManager.GenerateTwoFactorTokenAsync(user, provider);
            if (string.IsNullOrEmpty(code))
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_SYSTEM));
                return response;
            }
            response.Item = code;
            return response;
        }

        /// <summary>
        /// Add to role.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public IResponse AddToRole(string userStorageKey, string roleName)
        {
            return AddToRoleAsync(userStorageKey, roleName).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Add to role.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<IResponse> AddToRoleAsync(string userStorageKey, string roleName)
        {
            var response = new Response();
            var user = await _userManager.FindByIdAsync(userStorageKey);
            if (user == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                return response;
            }
            var result = await _userManager.AddToRoleAsync(user, roleName);
            response.CopyFrom(result);
            return response;
        }

        /// <summary>
        /// Create a user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public IResponseItem<ApplicationUserDto> Create(ApplicationUserDto user, string password)
        {
            return CreateAsync(user, password).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Create a user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<IResponseItem<ApplicationUserDto>> CreateAsync(ApplicationUserDto user, string password)
        {
            var response = new ResponseItem<ApplicationUserDto>();
            var appUser = _mapper.Map<ApplicationUser>(user);
            appUser.SecurityStamp = Guid.NewGuid().ToString();
            var respUser = await _userManager.CreateAsync(appUser, password);
            response.CopyFrom(respUser);
            if (response.Success)
                response.Item = _mapper.Map<ApplicationUserDto>(appUser);
            return response;
        }

        /// <summary>
        /// Add claim.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="claim"></param>
        /// <returns></returns>
        public IResponse AddClaim(string userStorageKey, Claim claim)
        {
            return AddClaimAsync(userStorageKey, claim).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Add claim.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="claim"></param>
        /// <returns></returns>
        public async Task<IResponse> AddClaimAsync(string userStorageKey, Claim claim)
        {
            var response = new Response();
            var user = await _userManager.FindByIdAsync(userStorageKey);
            if (user == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                return response;
            }

            var result = await _userManager.AddClaimAsync(user, claim);
            response.CopyFrom(result);
            return response;
        }

        /// <summary>
        /// Generate email confirmation token.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <returns></returns>
        public IResponseItem<string> GenerateEmailConfirmationToken(string userStorageKey)
        {
            return GenerateEmailConfirmationTokenAsync(userStorageKey).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Generate email confirmation token.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <returns></returns>
        public async Task<IResponseItem<string>> GenerateEmailConfirmationTokenAsync(string userStorageKey)
        {
            var response = new ResponseItem<string>();
            var user = await _userManager.FindByIdAsync(userStorageKey);
            if (user == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                return response;
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            if (string.IsNullOrEmpty(code))
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_SYSTEM));
                return response;
            }
            response.Item = code;
            return response;
        }

        /// <summary>
        /// Sign out
        /// </summary>
        /// <returns></returns>
        public IResponse SignOut()
        {
            return SignOutAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Sign out
        /// </summary>
        /// <returns></returns>
        public async Task<IResponse> SignOutAsync()
        {
            await _signInManager.SignOutAsync();
            return new Response();
        }

        /// <summary>
        /// Get users in role.
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public IResponseList<ApplicationUserDto> GetUsersInRole(string roleName)
        {
            return GetUsersInRoleAsync(roleName).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get users in role.
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<IResponseList<ApplicationUserDto>> GetUsersInRoleAsync(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            var response = new ResponseList<ApplicationUserDto>();
            response.List = _mapper.Map<List<ApplicationUserDto>>(users);
            return response;
        }

        /// <summary>
        /// Signin
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="isPersistent"></param>
        /// <param name="authenticationMethod"></param>
        /// <returns></returns>
        public IResponse SignIn(string userStorageKey, bool isPersistent, string authenticationMethod = null)
        {
            return SignInAsync(userStorageKey, isPersistent, authenticationMethod).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Signin
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="isPersistent"></param>
        /// <param name="authenticationMethod"></param>
        /// <returns></returns>
        public async Task<IResponse> SignInAsync(string userStorageKey, bool isPersistent, string authenticationMethod = null)
        {
            var response = new Response();
            var user = await _userManager.FindByIdAsync(userStorageKey);
            if (user == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                return response;
            }
            await _signInManager.SignInAsync(user, isPersistent, authenticationMethod);
            return response;
        }

        /// <summary>
        /// Password sign in.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="isPersistent"></param>
        /// <returns></returns>
        public IResponseItem<ApplicationSigninResult> PasswordSignIn(string email, string password, bool isPersistent)
        {
            return PasswordSignInAsync(email, password, isPersistent).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Password sign in.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="isPersistent"></param>
        /// <returns></returns>
        public async Task<IResponseItem<ApplicationSigninResult>> PasswordSignInAsync(string email, string password, bool isPersistent)
        {
            var response = new ResponseItem<ApplicationSigninResult>();
            response.Item = new ApplicationSigninResult();
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                return response;
            }

            response.Item.User = _mapper.Map<ApplicationUserDto>(user);
            response.Item.SignInResult = await _signInManager.PasswordSignInAsync(
                email,
                password,
                isPersistent,
                user.LockoutEnabled);
            if (!response.Item.SignInResult.Succeeded)
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_SECURITY));
            return response;
        }

        /// <summary>
        /// Get valid 2FA providers.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <returns></returns>
        public IResponseList<string> GetValidTwoFactorProviders(string userStorageKey)
        {
            return GetValidTwoFactorProvidersAsync(userStorageKey).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get valid 2FA providers.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <returns></returns>
        public async Task<IResponseList<string>> GetValidTwoFactorProvidersAsync(string userStorageKey)
        {
            var response = new ResponseList<string>();
            var user = await _userManager.FindByIdAsync(userStorageKey);
            if (user == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                return response;
            }
            var list = await _userManager.GetValidTwoFactorProvidersAsync(user);
            if (list != null)
                response.List.AddRange(list);
            return response;
        }

        /// <summary>
        /// Get 2FA sign in code.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="code"></param>
        /// <param name="isPersistent"></param>
        /// <param name="rememberBrowser"></param>
        /// <returns></returns>
        public IResponseItem<SignInResult> TwoFactorSignIn(string provider, string code, bool isPersistent, bool rememberBrowser)
        {
            return TwoFactorSignInAsync(provider, code, isPersistent, rememberBrowser).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 2FA sign in
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="code"></param>
        /// <param name="isPersistent"></param>
        /// <param name="rememberBrowser"></param>
        /// <returns></returns>
        public async Task<IResponseItem<SignInResult>> TwoFactorSignInAsync(string provider, string code, bool isPersistent, bool rememberBrowser)
        {
            var response = new ResponseItem<SignInResult>();
            //var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            //if (user == null)
            //{
            //    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
            //    return response;
            //}
            response.Item = await _signInManager.TwoFactorSignInAsync(
                provider,
                code,
                isPersistent,
                rememberBrowser);
            if (!response.Item.Succeeded)
                response.AddMessage(ResponseMessage.CreateError("Invalid Login Attempt"));
            return response;
        }

        /// <summary>
        /// Verify password.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public IResponseItem<ApplicationUserDto> VerifyPassword(string email, string password)
        {
            return VerifyPasswordAsync(email, password).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Verify password.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<IResponseItem<ApplicationUserDto>> VerifyPasswordAsync(string email, string password)
        {
            var response = new ResponseItem<ApplicationUserDto>();
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                return response;
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_SECURITY));
                return response;
            }

            var result = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result != PasswordVerificationResult.Success)
            {
                if (user.LockoutEnabled)
                    await _userManager.AccessFailedAsync(user);

                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_SECURITY));
                return response;
            }
            response.Item = _mapper.Map<ApplicationUserDto>(user);
            return response;
        }
    }
}