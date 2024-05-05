namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is a storage repository for the notification message domain object.
    /// </summary>
    public interface IAuditUserStorageRepository : IStorageRepository<AuditUser>
    {
    }
}