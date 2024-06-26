﻿namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an event when a user changes their password.
    /// </summary>
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
}