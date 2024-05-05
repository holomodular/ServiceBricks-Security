namespace ServiceBricks.Security
{
    /// <summary>
    /// This is an role.
    /// </summary>
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
}