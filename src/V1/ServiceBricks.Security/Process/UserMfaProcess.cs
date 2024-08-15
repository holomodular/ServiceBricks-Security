namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a process when a user needs MFA.
    /// </summary>
    public partial class UserMfaProcess : DomainProcess
    {
        public UserMfaProcess(string selectedProvider)
        {
            SelectedProvider = selectedProvider;
        }

        public virtual string SelectedProvider { get; set; }
    }
}