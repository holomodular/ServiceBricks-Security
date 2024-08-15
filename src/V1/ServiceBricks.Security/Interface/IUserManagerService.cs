using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a service for managing users.
    /// </summary>
    public partial interface IUserManagerService
    {
        /// <summary>
        /// This method is used to reset a user's password.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="code"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        IResponse ResetPassword(string userStorageKey, string code, string password);

        /// <summary>
        /// This method is used to reset a user's password.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="code"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<IResponse> ResetPasswordAsync(string userStorageKey, string code, string password);

        /// <summary>
        /// Find a user by email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        IResponseItem<UserDto> FindByEmail(string email);

        /// <summary>
        /// Find a user by email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<IResponseItem<UserDto>> FindByEmailAsync(string email);

        /// <summary>
        /// Find a user by id.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <returns></returns>
        IResponseItem<UserDto> FindById(string userStorageKey);

        /// <summary>
        /// Find a user by id.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <returns></returns>
        Task<IResponseItem<UserDto>> FindByIdAsync(string userStorageKey);

        /// <summary>
        /// Confirm a user's email.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        IResponse ConfirmEmail(string userStorageKey, string code);

        /// <summary>
        /// Confirm a user's email.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<IResponse> ConfirmEmailAsync(string userStorageKey, string code);

        /// <summary>
        /// Generate a password reset token.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <returns></returns>
        IResponseItem<string> GeneratePasswordResetToken(string userStorageKey);

        /// <summary>
        /// Generate a password reset token.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <returns></returns>
        Task<IResponseItem<string>> GeneratePasswordResetTokenAsync(string userStorageKey);

        /// <summary>
        /// Change a user's password.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        IResponse ChangePassword(string userStorageKey, string oldPassword, string newPassword);

        /// <summary>
        /// Change a user's password.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        Task<IResponse> ChangePasswordAsync(string userStorageKey, string oldPassword, string newPassword);

        /// <summary>
        /// Refresh the sign-in for a user.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <returns></returns>
        IResponse RefreshSignIn(string userStorageKey);

        /// <summary>
        /// Refresh the sign-in for a user.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <returns></returns>
        Task<IResponse> RefreshSignInAsync(string userStorageKey);

        /// <summary>
        /// Get the user that is currently signed in.
        /// </summary>
        /// <returns></returns>
        IResponseItem<UserDto> GetTwoFactorAuthenticationUser();

        /// <summary>
        /// Get the user that is currently signed in.
        /// </summary>
        /// <returns></returns>
        Task<IResponseItem<UserDto>> GetTwoFactorAuthenticationUserAsync();

        /// <summary>
        /// Generate a two-factor token.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        IResponseItem<string> GenerateTwoFactorToken(string userStorageKey, string provider);

        /// <summary>
        /// Generate a two-factor token.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        Task<IResponseItem<string>> GenerateTwoFactorTokenAsync(string userStorageKey, string provider);

        /// <summary>
        /// Add a user to a role.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        IResponse AddToRole(string userStorageKey, string roleName);

        /// <summary>
        /// Add a user to a role.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        Task<IResponse> AddToRoleAsync(string userStorageKey, string roleName);

        /// <summary>
        /// Create a user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        IResponseItem<UserDto> Create(UserDto user, string password);

        /// <summary>
        /// Create a user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<IResponseItem<UserDto>> CreateAsync(UserDto user, string password);

        /// <summary>
        /// Add a claim to a user.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="claim"></param>
        /// <returns></returns>
        IResponse AddClaim(string userStorageKey, Claim claim);

        /// <summary>
        /// Add a claim to a user.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="claim"></param>
        /// <returns></returns>
        Task<IResponse> AddClaimAsync(string userStorageKey, Claim claim);

        /// <summary>
        /// Generate an email confirmation token.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <returns></returns>
        IResponseItem<string> GenerateEmailConfirmationToken(string userStorageKey);

        /// <summary>
        /// Generate an email confirmation token.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <returns></returns>
        Task<IResponseItem<string>> GenerateEmailConfirmationTokenAsync(string userStorageKey);

        /// <summary>
        /// Sign out the current user.
        /// </summary>
        /// <returns></returns>
        IResponse SignOut();

        /// <summary>
        /// Sign out the current user.
        /// </summary>
        /// <returns></returns>
        Task<IResponse> SignOutAsync();

        /// <summary>
        /// Sign in a user.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="isPersistent"></param>
        /// <param name="authenticationMethod"></param>
        /// <returns></returns>
        IResponse SignIn(string userStorageKey, bool isPersistent, string authenticationMethod = null);

        /// <summary>
        /// Sign in a user.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <param name="isPersistent"></param>
        /// <param name="authenticationMethod"></param>
        /// <returns></returns>
        Task<IResponse> SignInAsync(string userStorageKey, bool isPersistent, string authenticationMethod = null);

        /// <summary>
        /// Password sign in a user.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="isPersistent"></param>
        /// <returns></returns>
        IResponseItem<ApplicationSigninResult> PasswordSignIn(string email, string password, bool isPersistent);

        /// <summary>
        /// Password sign in a user.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="isPersistent"></param>
        /// <returns></returns>
        Task<IResponseItem<ApplicationSigninResult>> PasswordSignInAsync(string email, string password, bool isPersistent);

        /// <summary>
        /// Get users in a role.
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        IResponseList<UserDto> GetUsersInRole(string roleName);

        /// <summary>
        /// Get users in a role.
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        Task<IResponseList<UserDto>> GetUsersInRoleAsync(string roleName);

        /// <summary>
        /// Get a list of 2FA providers.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <returns></returns>
        IResponseList<string> GetValidTwoFactorProviders(string userStorageKey);

        /// <summary>
        /// Get a list of 2FA providers.
        /// </summary>
        /// <param name="userStorageKey"></param>
        /// <returns></returns>
        Task<IResponseList<string>> GetValidTwoFactorProvidersAsync(string userStorageKey);

        /// <summary>
        /// 2FA sign in.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="code"></param>
        /// <param name="isPersistent"></param>
        /// <param name="rememberBrowser"></param>
        /// <returns></returns>
        IResponseItem<SignInResult> TwoFactorSignIn(string provider, string code, bool isPersistent, bool rememberBrowser);

        /// <summary>
        /// 2FA sign in.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="code"></param>
        /// <param name="isPersistent"></param>
        /// <param name="rememberBrowser"></param>
        /// <returns></returns>
        Task<IResponseItem<SignInResult>> TwoFactorSignInAsync(string provider, string code, bool isPersistent, bool rememberBrowser);

        /// <summary>
        /// Verify a password.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        IResponseItem<UserDto> VerifyPassword(string email, string password);

        /// <summary>
        /// Verify a password.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<IResponseItem<UserDto>> VerifyPasswordAsync(string email, string password);
    }
}