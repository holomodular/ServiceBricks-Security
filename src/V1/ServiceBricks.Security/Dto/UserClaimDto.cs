namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a claim for a user.
    /// </summary>
    public partial class UserClaimDto : DataTransferObject
    {
        /// <summary>
        /// Gets or sets the primary key of the user associated with this claim.
        /// </summary>
        public virtual string UserStorageKey { get; set; }

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