namespace ServiceBricks.Security
{
    /// <summary>
    /// This associates a user to a role.
    /// </summary>
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
}