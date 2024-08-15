﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is a API service for the ApplicationUser domain object.
    /// </summary>
    public partial class UserManagerService : IUserManagerService
    {
        protected readonly IMapper _mapper;
        protected readonly IUserApiService _applicationUserApiService;
        protected readonly IUserClaimApiService _applicationUserClaimApiService;
        protected readonly IUserLoginApiService _applicationUserLoginApiService;
        protected readonly IUserRoleApiService _applicationUserRoleApiService;
        protected readonly IUserTokenApiService _applicationUserTokenApiService;
        protected readonly IRoleApiService _applicationRoleApiService;
        protected readonly IRoleClaimApiService _applicationRoleClaimApiService;
        protected readonly UserManager<ApplicationIdentityUser> _userManager;
        protected readonly SignInManager<ApplicationIdentityUser> _signInManager;

        /// <summary>
        /// Constructor.
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
            IUserApiService applicationUserApiService,
            IUserClaimApiService applicationUserClaimApiService,
            IUserLoginApiService applicationUserLoginApiService,
            IUserRoleApiService applicationUserRoleApiService,
            IUserTokenApiService applicationUserTokenApiService,
            IRoleApiService applicationRoleApiService,
            IRoleClaimApiService applicationRoleClaimApiService,
            UserManager<ApplicationIdentityUser> userManager,
            SignInManager<ApplicationIdentityUser> signInManager)
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
        public virtual IResponseItem<UserDto> FindByEmail(string email)
        {
            return FindByEmailAsync(email).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find a user by email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public virtual async Task<IResponseItem<UserDto>> FindByEmailAsync(string email)
        {
            var response = new ResponseItem<UserDto>();
            if (string.IsNullOrEmpty(email))
                return response;

            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
                response.Item = _mapper.Map<UserDto>(user);

            return response;
        }

        /// <summary>
        /// Find a user by id.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <returns></returns>
        public virtual IResponseItem<UserDto> FindById(string userStorageKey)
        {
            return FindByIdAsync(userStorageKey).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find a user by id.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <returns></returns>
        public virtual async Task<IResponseItem<UserDto>> FindByIdAsync(string userStorageKey)
        {
            var response = new ResponseItem<UserDto>();
            if (string.IsNullOrEmpty(userStorageKey))
                return response;

            var user = await _userManager.FindByIdAsync(userStorageKey);
            if (user != null)
                response.Item = _mapper.Map<UserDto>(user);

            return response;
        }

        /// <summary>
        /// Confirm a user's email.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public virtual IResponse ConfirmEmail(string userStorageKey, string code)
        {
            return ConfirmEmailAsync(userStorageKey, code).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Confirm a user's email.
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
        public virtual IResponseItem<string> GeneratePasswordResetToken(string userStorageKey)
        {
            return GeneratePasswordResetTokenAsync(userStorageKey).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Generate a password reset token.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <returns></returns>
        public virtual async Task<IResponseItem<string>> GeneratePasswordResetTokenAsync(string userStorageKey)
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
        /// Reset a user's password.
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
        /// Reset a user's password.
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
        /// Change a user's password.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="currentPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public virtual IResponse ChangePassword(string userStorageKey, string currentPassword, string newPassword)
        {
            return ChangePasswordAsync(userStorageKey, currentPassword, newPassword).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Change a user's password.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="currentPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public virtual async Task<IResponse> ChangePasswordAsync(string userStorageKey, string currentPassword, string newPassword)
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
        /// Refresh the sign in for a user.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <returns></returns>
        public virtual IResponse RefreshSignIn(string userStorageKey)
        {
            return RefreshSignInAsync(userStorageKey).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Refresh the sign in for a user.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <returns></returns>
        public virtual async Task<IResponse> RefreshSignInAsync(string userStorageKey)
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
        public virtual IResponseItem<UserDto> GetTwoFactorAuthenticationUser()
        {
            return GetTwoFactorAuthenticationUserAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get 2FA user.
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IResponseItem<UserDto>> GetTwoFactorAuthenticationUserAsync()
        {
            var response = new ResponseItem<UserDto>();
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                return response;
            }
            response.Item = _mapper.Map<UserDto>(user);
            return response;
        }

        /// <summary>
        /// Generate a 2FA token.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public virtual IResponseItem<string> GenerateTwoFactorToken(string userStorageKey, string provider)
        {
            return GenerateTwoFactorTokenAsync(userStorageKey, provider).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Generate a 2FA token.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public virtual async Task<IResponseItem<string>> GenerateTwoFactorTokenAsync(string userStorageKey, string provider)
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
        /// Add a user to a role.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public virtual IResponse AddToRole(string userStorageKey, string roleName)
        {
            return AddToRoleAsync(userStorageKey, roleName).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Add a user to a role.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public virtual async Task<IResponse> AddToRoleAsync(string userStorageKey, string roleName)
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
        public virtual IResponseItem<UserDto> Create(UserDto user, string password)
        {
            return CreateAsync(user, password).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Create a user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public virtual async Task<IResponseItem<UserDto>> CreateAsync(UserDto user, string password)
        {
            var response = new ResponseItem<UserDto>();
            var appUser = _mapper.Map<ApplicationIdentityUser>(user);
            var respUser = await _userManager.CreateAsync(appUser, password);
            appUser.SecurityStamp = Guid.NewGuid().ToString();
            response.CopyFrom(respUser);
            if (response.Success)
                response.Item = _mapper.Map<UserDto>(appUser);
            return response;
        }

        /// <summary>
        /// Add a claim to a user.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="claim"></param>
        /// <returns></returns>
        public virtual IResponse AddClaim(string userStorageKey, Claim claim)
        {
            return AddClaimAsync(userStorageKey, claim).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Add a claim to a user.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="claim"></param>
        /// <returns></returns>
        public virtual async Task<IResponse> AddClaimAsync(string userStorageKey, Claim claim)
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
        /// Generate a email confirmation token.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <returns></returns>
        public virtual IResponseItem<string> GenerateEmailConfirmationToken(string userStorageKey)
        {
            return GenerateEmailConfirmationTokenAsync(userStorageKey).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Generate a email confirmation token.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <returns></returns>
        public virtual async Task<IResponseItem<string>> GenerateEmailConfirmationTokenAsync(string userStorageKey)
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
        /// Sign out a user.
        /// </summary>
        /// <returns></returns>
        public virtual IResponse SignOut()
        {
            return SignOutAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Sign out a user.
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IResponse> SignOutAsync()
        {
            await _signInManager.SignOutAsync();
            return new Response();
        }

        /// <summary>
        /// Get users in a role.
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public virtual IResponseList<UserDto> GetUsersInRole(string roleName)
        {
            return GetUsersInRoleAsync(roleName).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get users in a role.
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public virtual async Task<IResponseList<UserDto>> GetUsersInRoleAsync(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            var response = new ResponseList<UserDto>();
            response.List = _mapper.Map<List<UserDto>>(users);
            return response;
        }

        /// <summary>
        /// Sign in a user.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="isPersistent"></param>
        /// <param name="authenticationMethod"></param>
        /// <returns></returns>
        public virtual IResponse SignIn(string userStorageKey, bool isPersistent, string authenticationMethod = null)
        {
            return SignInAsync(userStorageKey, isPersistent, authenticationMethod).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Sign in a user.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="isPersistent"></param>
        /// <param name="authenticationMethod"></param>
        /// <returns></returns>
        public virtual async Task<IResponse> SignInAsync(string userStorageKey, bool isPersistent, string authenticationMethod = null)
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
        /// Password sign in a user.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="isPersistent"></param>
        /// <returns></returns>
        public virtual IResponseItem<ApplicationSigninResult> PasswordSignIn(string email, string password, bool isPersistent)
        {
            return PasswordSignInAsync(email, password, isPersistent).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Password sign in a user.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="isPersistent"></param>
        /// <returns></returns>
        public virtual async Task<IResponseItem<ApplicationSigninResult>> PasswordSignInAsync(string email, string password, bool isPersistent)
        {
            var response = new ResponseItem<ApplicationSigninResult>();
            response.Item = new ApplicationSigninResult();
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                return response;
            }

            response.Item.User = _mapper.Map<UserDto>(user);
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
        public virtual IResponseList<string> GetValidTwoFactorProviders(string userStorageKey)
        {
            return GetValidTwoFactorProvidersAsync(userStorageKey).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get valid 2FA providers.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <returns></returns>
        public virtual async Task<IResponseList<string>> GetValidTwoFactorProvidersAsync(string userStorageKey)
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
        /// two factor sign in a user.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="code"></param>
        /// <param name="isPersistent"></param>
        /// <param name="rememberBrowser"></param>
        /// <returns></returns>
        public virtual IResponseItem<SignInResult> TwoFactorSignIn(string provider, string code, bool isPersistent, bool rememberBrowser)
        {
            return TwoFactorSignInAsync(provider, code, isPersistent, rememberBrowser).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Two factor sign in a user.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="code"></param>
        /// <param name="isPersistent"></param>
        /// <param name="rememberBrowser"></param>
        /// <returns></returns>
        public virtual async Task<IResponseItem<SignInResult>> TwoFactorSignInAsync(string provider, string code, bool isPersistent, bool rememberBrowser)
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
        /// Verify a user's password.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public virtual IResponseItem<UserDto> VerifyPassword(string email, string password)
        {
            return VerifyPasswordAsync(email, password).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Verify a user's password.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public virtual async Task<IResponseItem<UserDto>> VerifyPasswordAsync(string email, string password)
        {
            var response = new ResponseItem<UserDto>();
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
            response.Item = _mapper.Map<UserDto>(user);
            return response;
        }
    }
}