using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ServiceBricks.Security
{
    public interface IUserManagerService
    {
        IResponse ResetPassword(string userStorageKey, string code, string password);

        Task<IResponse> ResetPasswordAsync(string userStorageKey, string code, string password);

        IResponseItem<ApplicationUserDto> FindByEmail(string email);

        Task<IResponseItem<ApplicationUserDto>> FindByEmailAsync(string email);

        IResponseItem<ApplicationUserDto> FindById(string userStorageKey);

        Task<IResponseItem<ApplicationUserDto>> FindByIdAsync(string userStorageKey);

        IResponse ConfirmEmail(string userStorageKey, string code);

        Task<IResponse> ConfirmEmailAsync(string userStorageKey, string code);

        IResponseItem<string> GeneratePasswordResetToken(string userStorageKey);

        Task<IResponseItem<string>> GeneratePasswordResetTokenAsync(string userStorageKey);

        IResponse ChangePassword(string userStorageKey, string oldPassword, string newPassword);

        Task<IResponse> ChangePasswordAsync(string userStorageKey, string oldPassword, string newPassword);

        IResponse RefreshSignIn(string userStorageKey);

        Task<IResponse> RefreshSignInAsync(string userStorageKey);

        IResponseItem<ApplicationUserDto> GetTwoFactorAuthenticationUser();

        Task<IResponseItem<ApplicationUserDto>> GetTwoFactorAuthenticationUserAsync();

        IResponseItem<string> GenerateTwoFactorToken(string userStorageKey, string provider);

        Task<IResponseItem<string>> GenerateTwoFactorTokenAsync(string userStorageKey, string provider);

        IResponse AddToRole(string userStorageKey, string roleName);

        Task<IResponse> AddToRoleAsync(string userStorageKey, string roleName);

        IResponseItem<ApplicationUserDto> Create(ApplicationUserDto user, string password);

        Task<IResponseItem<ApplicationUserDto>> CreateAsync(ApplicationUserDto user, string password);

        IResponse AddClaim(string userStorageKey, Claim claim);

        Task<IResponse> AddClaimAsync(string userStorageKey, Claim claim);

        IResponseItem<string> GenerateEmailConfirmationToken(string userStorageKey);

        Task<IResponseItem<string>> GenerateEmailConfirmationTokenAsync(string userStorageKey);

        IResponse SignOut();

        Task<IResponse> SignOutAsync();

        IResponse SignIn(string userStorageKey, bool isPersistent, string authenticationMethod = null);

        Task<IResponse> SignInAsync(string userStorageKey, bool isPersistent, string authenticationMethod = null);

        IResponseItem<ApplicationSigninResult> PasswordSignIn(string email, string password, bool isPersistent);

        Task<IResponseItem<ApplicationSigninResult>> PasswordSignInAsync(string email, string password, bool isPersistent);

        IResponseList<ApplicationUserDto> GetUsersInRole(string roleName);

        Task<IResponseList<ApplicationUserDto>> GetUsersInRoleAsync(string roleName);

        IResponseList<string> GetValidTwoFactorProviders(string userStorageKey);

        Task<IResponseList<string>> GetValidTwoFactorProvidersAsync(string userStorageKey);

        IResponseItem<SignInResult> TwoFactorSignIn(string provider, string code, bool isPersistent, bool rememberBrowser);

        Task<IResponseItem<SignInResult>> TwoFactorSignInAsync(string provider, string code, bool isPersistent, bool rememberBrowser);

        IResponseItem<ApplicationUserDto> VerifyPassword(string email, string password);

        Task<IResponseItem<ApplicationUserDto>> VerifyPasswordAsync(string email, string password);
    }
}