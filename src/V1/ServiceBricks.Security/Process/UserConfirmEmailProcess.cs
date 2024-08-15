namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a process to confirm an email with a code.
    /// </summary>
    public partial class UserConfirmEmailProcess : DomainProcess
    {
        public UserConfirmEmailProcess(string userStorageKey, string code)
        {
            UserStorageKey = userStorageKey;
            Code = code;
        }

        public virtual string UserStorageKey { get; set; }
        public virtual string Code { get; set; }
    }
}