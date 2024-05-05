namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a user security data transfer object.
    /// </summary>
    public class AuditUserDto : DataTransferObject
    {
        public DateTimeOffset CreateDate { get; set; }
        public string UserStorageKey { get; set; }
        public string IPAddress { get; set; }
        public string UserAgent { get; set; }
        public string AuditName { get; set; }
        public string Data { get; set; }
    }
}