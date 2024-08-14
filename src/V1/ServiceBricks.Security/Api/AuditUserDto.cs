namespace ServiceBricks.Security
{
    /// <summary>
    /// This allows auditing of user actions.
    /// </summary>
    public partial class AuditUserDto : DataTransferObject
    {
        /// <summary>
        /// The create date and time.
        /// </summary>
        public virtual DateTimeOffset CreateDate { get; set; }

        /// <summary>
        /// The user storage key.
        /// </summary>
        public virtual string UserStorageKey { get; set; }

        /// <summary>
        /// The IP address of the user.
        /// </summary>
        public virtual string IPAddress { get; set; }

        /// <summary>
        /// The user agent of the user.
        /// </summary>
        public virtual string UserAgent { get; set; }

        /// <summary>
        /// The audit name.
        /// </summary>
        public virtual string AuditName { get; set; }

        /// <summary>
        /// The data.
        /// </summary>
        public virtual string Data { get; set; }
    }
}