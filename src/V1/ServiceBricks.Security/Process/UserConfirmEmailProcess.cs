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

        public string UserStorageKey { get; set; }
        public string Code { get; set; }
    }
}