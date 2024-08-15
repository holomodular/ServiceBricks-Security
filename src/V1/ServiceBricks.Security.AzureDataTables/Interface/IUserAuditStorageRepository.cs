namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is a storage repository for the AuditUser domain object.
    /// </summary>
    public partial interface IUserAuditStorageRepository : IStorageRepository<UserAudit>
    {
    }
}