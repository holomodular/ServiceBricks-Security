namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an event when a user needs MFA.
    /// </summary>
    public class UserMfaProcess : DomainProcess
    {
        public UserMfaProcess(string selectedProvider)
        {
            SelectedProvider = selectedProvider;
        }

        public string SelectedProvider { get; set; }
    }
}