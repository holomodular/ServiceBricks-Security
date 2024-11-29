![ServiceBricks Logo](https://github.com/holomodular/ServiceBricks/blob/main/Logo.png)  

[![NuGet version](https://badge.fury.io/nu/ServiceBricks.Security.svg)](https://badge.fury.io/nu/ServiceBricks.Security.Microservice)
![badge](https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/holomodular-support/0f40e020ebb5096a51bc7ccd02a6e06e/raw/servicebrickssecurity-codecoverage.json)
[![License: MIT](https://img.shields.io/badge/License-MIT-389DA0.svg)](https://opensource.org/licenses/MIT)

# ServiceBricks Security Microservice

## Overview

This repository contains a security microservice built using the ServiceBricks foundation.
The security microservice is responsible for application security and user management built using the Microsoft ASP.NET Core Identity (v3) provider.
It also stores auditing events for users.

The NuGet package ServiceBricks.Security.Member that allows any ServiceBricks hosted microservice application to become a security member using JWT tokens.

## Supported Providers
* Microsoft ASP.NET Core Identity (v3)

### ServiceBricks.Security.Member
Install the NuGet package ServiceBricks.Security.Member to your web application and add the ServiceBricks:Security:Token application settings. 
If the settings match the security microservices application settings, it will be joined as a member of the platform using JWT bearer token membership.

Make sure to add the following line to startup.
```csharp
services.AddServiceBricksSecurityMember(Configuration);
```

## Data Transfer Objects


### AuditUserDto - Admin Policy
Audit user events.

```csharp
public class AuditUserDto : DataTransferObject
{
    public DateTimeOffset CreateDate { get; set; }
    public string UserStorageKey { get; set; }
    public string IPAddress { get; set; }
    public string UserAgent { get; set; }
    public string AuditName { get; set; }
    public string Data { get; set; }
}

```

#### Business Rules

* DomainCreateDateRule - CreateDate property


### ApplicationUserDto - Admin Policy
Application user object.

```csharp

public partial class ApplicationUserDto : DataTransferObject
{
    /// <summary>
    /// Gets or sets the user name for this user.
    /// </summary>
    public virtual string UserName { get; set; }

    /// <summary>
    /// Gets or sets the normalized user name for this user.
    /// </summary>
    public virtual string NormalizedUserName { get; set; }

    /// <summary>
    /// Gets or sets the email address for this user.
    /// </summary>
    public virtual string Email { get; set; }

    /// <summary>
    /// Gets or sets the normalized email address for this user.
    /// </summary>
    public virtual string NormalizedEmail { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating if a user has confirmed their email address.
    /// </summary>
    /// <value>True if the email address has been confirmed, otherwise false.</value>
    public virtual bool EmailConfirmed { get; set; }

    /// <summary>
    /// Gets or sets a salted and hashed representation of the password for this user.
    /// </summary>
    public virtual string PasswordHash { get; set; }

    /// <summary>
    /// A random value that must change whenever a users credentials change (password changed, login removed)
    /// </summary>
    public virtual string SecurityStamp { get; set; }

    /// <summary>
    /// A random value that must change whenever a user is persisted to the store
    /// </summary>
    public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets a telephone number for the user.
    /// </summary>
    public virtual string PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating if a user has confirmed their telephone address.
    /// </summary>
    /// <value>True if the telephone number has been confirmed, otherwise false.</value>
    public virtual bool PhoneNumberConfirmed { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating if two factor authentication is enabled for this user.
    /// </summary>
    /// <value>True if 2fa is enabled, otherwise false.</value>
    public virtual bool TwoFactorEnabled { get; set; }

    /// <summary>
    /// Gets or sets the date and time, in UTC, when any user lockout ends.
    /// </summary>
    /// <remarks>
    /// A value in the past means the user is not locked out.
    /// </remarks>
    public virtual DateTimeOffset? LockoutEnd { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating if the user could be locked out.
    /// </summary>
    /// <value>True if the user could be locked out, otherwise false.</value>
    public virtual bool LockoutEnabled { get; set; }

    /// <summary>
    /// Gets or sets the number of failed login attempts for the current user.
    /// </summary>
    public virtual int AccessFailedCount { get; set; }

    public virtual DateTimeOffset CreateDate { get; set; }
    public virtual DateTimeOffset UpdateDate { get; set; }
}

```

#### Business Rules

* DomainCreateUpdateDateRule - CreateDate and UpdateDate property


### ApplicationRoleDto - Admin Policy
Application role object.

```csharp

public partial class ApplicationRoleDto : DataTransferObject
{
    /// <summary>
    /// Gets or sets the name for this role.
    /// </summary>
    public virtual string Name { get; set; }

    /// <summary>
    /// Gets or sets the normalized name for this role.
    /// </summary>
    public virtual string NormalizedName { get; set; }

    /// <summary>
    /// A random value that should change whenever a role is persisted to the store
    /// </summary>
    public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
}

```

#### Business Rules
None



### ApplicationRoleClaimDto - Admin Policy
Stores claims associated to an application role.

```csharp
public partial class ApplicationRoleClaimDto : DataTransferObject
{
    /// <summary>
    /// Gets or sets the of the primary key of the role associated with this claim.
    /// </summary>
    public virtual string RoleStorageKey { get; set; }

    /// <summary>
    /// Gets or sets the claim type for this claim.
    /// </summary>
    public virtual string ClaimType { get; set; }

    /// <summary>
    /// Gets or sets the claim value for this claim.
    /// </summary>
    public virtual string ClaimValue { get; set; }
}

```

#### Business Rules

None

### ApplicationUserRoleDto - Admin Policy
Stores relationship between an application user and application role.

```csharp
public partial class ApplicationUserRoleDto : DataTransferObject
{
    /// <summary>
    /// Gets or sets the primary key of the user that is linked to a role.
    /// </summary>
    public virtual string UserStorageKey { get; set; }

    /// <summary>
    /// Gets or sets the primary key of the role that is linked to the user.
    /// </summary>
    public virtual string RoleStorageKey { get; set; }
}

```

#### Business Rules

None



### ApplicationUserClaimDto - Admin Policy
Stores claims associated to an application user.

```csharp
public partial class ApplicationUserClaimDto : DataTransferObject
{
    /// <summary>
    /// Gets or sets the primary key of the user associated with this claim.
    /// </summary>
    public virtual string UserStorageKey { get; set; }

    /// <summary>
    /// Gets or sets the claim type for this claim.
    /// </summary>
    public virtual string ClaimType { get; set; }

    /// <summary>
    /// Gets or sets the claim value for this claim.
    /// </summary>
    public virtual string ClaimValue { get; set; }
}

```

#### Business Rules

None



### ApplicationUserLoginDto - Admin Policy
Stores logins associated to an application user.

```csharp
public partial class ApplicationUserLoginDto : DataTransferObject
{
    /// <summary>
    /// Gets or sets the login provider for the login (e.g. facebook, google)
    /// </summary>
    public virtual string LoginProvider { get; set; }

    /// <summary>
    /// Gets or sets the unique provider identifier for this login.
    /// </summary>
    public virtual string ProviderKey { get; set; }

    /// <summary>
    /// Gets or sets the friendly name used in a UI for this login.
    /// </summary>
    public virtual string ProviderDisplayName { get; set; }

    /// <summary>
    /// Gets or sets the primary key of the user associated with this login.
    /// </summary>
    public virtual string UserStorageKey { get; set; }
}

```

#### Business Rules

None



### ApplicationUserTokenDto - Admin Policy
Stores login tokens associated to an application user.

```csharp
public partial class ApplicationUserTokenDto : DataTransferObject
{
    /// <summary>
    /// Gets or sets the primary key of the user that the token belongs to.
    /// </summary>
    public virtual string UserStorageKey { get; set; }

    /// <summary>
    /// Gets or sets the LoginProvider this token is from.
    /// </summary>
    public virtual string LoginProvider { get; set; }

    /// <summary>
    /// Gets or sets the name of the token.
    /// </summary>
    public virtual string Name { get; set; }

    /// <summary>
    /// Gets or sets the token value.
    /// </summary>
    public virtual string Value { get; set; }
}

```

#### Business Rules

None


## Background Tasks and Timers
None

## Events
None

## Processes

### SendConfirmEmailProcess
This process is associated to the [SendConfirmEmailRule](https://github.com/holomodular/ServiceBricks-Security/blob/main/src/V1/ServiceBricks.Security/Rule/SendConfirmEmailRule.cs) Business Rule.

The business rule will first create an email and replace the callback url (with confirmation code) inside of the text. It will then send a service bus broadcast message for CreateApplicationEmail. The notification microservice will store this message and it will be sent when it is processed on a background task using a timer.

```csharp

public class SendConfirmEmailProcess : DomainProcess
{
    public SendConfirmEmailProcess(ApplicationUserDto applicationUser, string callbackUrl)
    {
        ApplicationUser = applicationUser;
        CallbackUrl = callbackUrl;
    }

    public ApplicationUserDto ApplicationUser { get; set; }
    public string CallbackUrl { get; set; }
}

```


### SendResetPasswordEmailProcess
This process is associated to the [SendResetPasswordEmailRule](https://github.com/holomodular/ServiceBricks-Security/blob/main/src/V1/ServiceBricks.Security/Rule/SendResetPasswordEmailRule.cs) Business Rule.

The business rule will first create an email and replace the callback url (with confirmation code) inside of the Reset Email text. It will then send a service bus broadcast message for CreateApplicationEmail. The notification microservice will store this message and it will be sent when it is processed on a background task using a timer.

```csharp

public class SendResetPasswordEmailProcess : DomainProcess
{
    public SendResetPasswordEmailProcess(ApplicationUserDto applicationUser, string callbackUrl)
    {
        ApplicationUser = applicationUser;
        CallbackUrl = callbackUrl;
    }

    public ApplicationUserDto ApplicationUser { get; set; }
    public string CallbackUrl { get; set; }
}

```


### UserConfirmEmailProcess
This process is associated to the [UserConfirmEmailRule](https://github.com/holomodular/ServiceBricks-Security/blob/main/src/V1/ServiceBricks.Security/Rule/UserConfirmEmailRule.cs) Business Rule.

The business rule will validate a confirmation code with a user.

```csharp

 public class UserConfirmEmailProcess : DomainProcess
{
    public UserConfirmEmailProcess(string userStorageKey, string code)
    {
        UserStorageKey = userStorageKey;
        Code = code;
    }

    public string UserStorageKey { get; set; }
    public string Code { get; set; }
}

```

### UserForgotPasswordProcess
This process is associated to the [UserForgotPasswordRule](https://github.com/holomodular/ServiceBricks-Security/blob/main/src/V1/ServiceBricks.Security/Rule/UserForgotPasswordRule.cs) Business Rule.

The business rule will create a reset token for a user, replace the callback url (with reset code) inside of the Forgot Password email text. It will then send a service bus broadcast message for CreateApplicationEmail. The notification microservice will store this message and it will be sent when it is processed on a background task using a timer.

```csharp

public class UserForgotPasswordProcess : DomainProcess<string>
{
    public UserForgotPasswordProcess(string userStorageKey)
    {
        DomainObject = userStorageKey;
    }
}

```


### UserInvalidPasswordProcess
This process is associated to the [UserInvalidPasswordRule](https://github.com/holomodular/ServiceBricks-Security/blob/main/src/V1/ServiceBricks.Security/Rule/UserInvalidPasswordRule.cs) Business Rule.

The business rule will create an audit message for the user denoting an invalid password login request.

```csharp

public class UserInvalidPasswordProcess : DomainProcess
{
    public UserInvalidPasswordProcess(string userStorageKey, string email)
    {
        UserStorageKey = userStorageKey;
        Email = email;
    }

    public string Email { get; set; }
    public string UserStorageKey { get; set; }
}

```


### UserLoginProcess
This process is associated to the [UserLoginRule](https://github.com/holomodular/ServiceBricks-Security/blob/main/src/V1/ServiceBricks.Security/Rule/UserLoginRule.cs) Business Rule.

The business rule will attempt to signin a user and create an audituser message.

```csharp

public class UserLoginProcess : DomainProcess
{
    public UserLoginProcess(
        string email,
        string password,
        bool rememberMe)
    {
        Email = email;
        Password = password;
        RememberMe = rememberMe;
    }

    public string Email { get; set; }
    public string Password { get; set; }

    public bool RememberMe { get; set; }

    public ApplicationSigninResult ApplicationSigninResult { get; set; }
}

```

### UserLogoutProcess
This process is associated to the [UserLogoutRule](https://github.com/holomodular/ServiceBricks-Security/blob/main/src/V1/ServiceBricks.Security/Rule/UserLogoutRule.cs) Business Rule.

The business rule will logout a user and create an audituser message.

```csharp

public class UserLogoutProcess : DomainProcess
{
    public UserLogoutProcess(string userStorageKey)
    {
        UserStorageKey = userStorageKey;
    }

    public string UserStorageKey { get; set; }
}

```


### UserMfaProcess
This process is associated to the [UserMFARule](https://github.com/holomodular/ServiceBricks-Security/blob/main/src/V1/ServiceBricks.Security/Rule/UserMFARule.cs) Business Rule.

The business rule will send the mult-factor authentication code to the user.

```csharp

public class UserMfaProcess : DomainProcess
{
    public UserMfaProcess(string selectedProvider)
    {
        SelectedProvider = selectedProvider;
    }

    public string SelectedProvider { get; set; }
}

```


### UserPasswordChangeProcess
This process is associated to the [UserPasswordChangeRule](https://github.com/holomodular/ServiceBricks-Security/blob/main/src/V1/ServiceBricks.Security/Rule/UserPasswordChangeRule.cs) Business Rule.

The business rule will send attempt to change a user's password.

```csharp

public class UserPasswordChangeProcess : DomainProcess
{
    public UserPasswordChangeProcess(string userStorageKey, string oldPassword, string newPassword)
    {
        UserStorageKey = userStorageKey;
        OldPassword = oldPassword;
        NewPassword = newPassword;
    }

    public string UserStorageKey { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}

```

### UserPasswordResetProcess
This process is associated to the [UserPasswordResetRule](https://github.com/holomodular/ServiceBricks-Security/blob/main/src/V1/ServiceBricks.Security/Rule/UserPasswordResetRule.cs) Business Rule.

The business rule will attempt to reset a user's password with the reset token.

```csharp

public class UserPasswordResetProcess : DomainProcess
{
    public UserPasswordResetProcess(string email, string password, string code)
    {
        Email = email;
        Password = password;
        Code = code;
    }

    public string Email { get; set; }
    public string Password { get; set; }
    public string Code { get; set; }
}
```


### UserProfileChangeProcess
This process is associated to the [UserProfileChangeRule](https://github.com/holomodular/ServiceBricks-Security/blob/main/src/V1/ServiceBricks.Security/Rule/UserProfileChangeRule.cs) Business Rule.

The business rule will attempt to update a user's profile.

```csharp

public class UserProfileChangeProcess : DomainProcess
{
    public UserProfileChangeProcess(string userStorageKey)
    {
        UserStorageKey = userStorageKey;
    }

    public string UserStorageKey { get; set; }
}

```

### UserRegisterAdminProcess
This process is associated to the [UserRegisterAdminRule](https://github.com/holomodular/ServiceBricks-Security/blob/main/src/V1/ServiceBricks.Security/Rule/UserRegisterAdminRule.cs) Business Rule.

The business rule will attempt to regiser a user as an admin user.

```csharp

public class UserRegisterAdminProcess : UserRegisterProcess
{
    public UserRegisterAdminProcess(
        string email,
        string password) : base(email, password)
    {
    }
}

```

### UserRegisterProcess
This process is associated to the [UserRegisterRule](https://github.com/holomodular/ServiceBricks-Security/blob/main/src/V1/ServiceBricks.Security/Rule/UserRegisterRule.cs) Business Rule.

The business rule will attempt to regiser a user.

```csharp

public class UserRegisterProcess : DomainProcess
{
    public UserRegisterProcess(
        string email,
        string password,
        bool createEmail = true,
        bool emailConfirmed = false)
    {
        Email = email;
        Password = password;
        CreateEmail = createEmail;
        EmailConfirmed = emailConfirmed;
    }

    public string Email { get; set; }
    public string Password { get; set; }
    public bool CreateEmail { get; set; }
    public bool EmailConfirmed { get; set; }
}

```

### UserResendConfirmationProcess
This process is associated to the [UserResendConfirmationProcessRule](https://github.com/holomodular/ServiceBricks-Security/blob/main/src/V1/ServiceBricks.Security/Rule/UserResendConfirmationProcessRule.cs) Business Rule.

The business rule will send the user another email with a confirmation code.

```csharp

public class UserResendConfirmationProcess : DomainProcess
{
    public UserResendConfirmationProcess(string userStorageKey)
    {
        UserStorageKey = userStorageKey;
    }

    public string UserStorageKey { get; set; }
}

```


## Service Bus
No Broadcast Messages

* This microservice sends CreateApplicationEmailBroadcast messages to the service bus.

## Additional


## Application Settings

```json

{
  // ServiceBricks Settings
  "ServiceBricks":{

    // Security Microservice Settings
    "Security": {

      // The callback url of the server if linkgenerator is not available
      "CallbackUrl": "https://localhost:7000",

      // JWT token settings
      "Token": {
        "ValidIssuer": "https://localhost:7000",
        "ValidAudience": "ServiceBricks",
        "ExpireMinutes": 1440,

        // Change this key in production!
        "SecretKey": "1111111111111111111111111111111111111111111111111111111111111111"
      }
    },
  }
}

```

# About ServiceBricks

ServiceBricks is the cornerstone for building a microservices foundation.
Visit http://ServiceBricks.com to learn more.

