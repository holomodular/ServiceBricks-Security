namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is a storage repository for the notification message domain object.
    /// </summary>
    public partial interface IAuditUserStorageRepository : IStorageRepository<AuditUser>
    {
    }
}