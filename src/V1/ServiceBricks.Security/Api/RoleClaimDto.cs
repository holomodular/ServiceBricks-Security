namespace ServiceBricks.Security
{
    /// <summary>
    /// This associated a role to a claim.
    /// </summary>
    public partial class RoleClaimDto : DataTransferObject
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
}