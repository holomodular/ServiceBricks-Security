using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is a mapper profile for the AuditUser domain object.
    /// </summary>
    public partial class UserAuditMappingProfile
    {
        /// <summary>
        /// Register the mapping
        /// </summary>
        public static void Register(IMapperRegistry registry)
        {
            registry.Register<UserAudit, UserAuditDto>(
                (s, d) =>
                {
                    d.AuditType = s.AuditType;
                    d.CreateDate = s.CreateDate;
                    if (d.CreateDate < StorageAzureDataTablesConstants.DATETIMEOFFSET_MINDATE)
                        d.CreateDate = StorageAzureDataTablesConstants.DATETIMEOFFSET_MINDATE;
                    d.Data = s.Data;
                    d.IPAddress = s.IPAddress;
                    d.RequestHeaders = s.RequestHeaders;
                    d.UserStorageKey = s.UserId.ToString();
                    var reverseDate = DateTimeOffset.MaxValue.Ticks - d.CreateDate.Ticks;
                    d.StorageKey =
                        s.UserId.ToString() +
                        StorageAzureDataTablesConstants.STORAGEKEY_DELIMITER +
                        reverseDate.ToString("d19") +
                        StorageAzureDataTablesConstants.KEY_DELIMITER +
                        s.Key.ToString();
                });

            registry.Register<UserAuditDto, UserAudit>(
                (s, d) =>
                {
                    if (!string.IsNullOrEmpty(s.StorageKey))
                    {
                        string[] split = s.StorageKey.Split(StorageAzureDataTablesConstants.STORAGEKEY_DELIMITER);
                        if (split.Length >= 1)
                        {
                            Guid tempUserId;
                            if (Guid.TryParse(split[0], out tempUserId))
                                d.UserId = tempUserId;
                        }
                        if (split.Length >= 2)
                        {
                            string[] splitRowKey = split[1].Split(StorageAzureDataTablesConstants.KEY_DELIMITER);
                            if (splitRowKey.Length >= 1)
                            {
                                long tempReverseTicks;
                                if (long.TryParse(splitRowKey[0], out tempReverseTicks))
                                {
                                    long originalTicks = DateTimeOffset.MaxValue.Ticks - tempReverseTicks;
                                    d.CreateDate = new DateTimeOffset(originalTicks, TimeSpan.Zero);
                                }
                            }
                            if (splitRowKey.Length >= 2)
                            {
                                Guid tempKey;
                                if (Guid.TryParse(splitRowKey[1], out tempKey))
                                    d.Key = tempKey;
                            }
                        }
                    }
                    else
                    {
                        Guid tempUserId;
                        if (Guid.TryParse(s.UserStorageKey, out tempUserId))
                            d.UserId = tempUserId;
                        d.CreateDate = s.CreateDate;
                        if (d.CreateDate < StorageAzureDataTablesConstants.DATETIMEOFFSET_MINDATE)
                            d.CreateDate = StorageAzureDataTablesConstants.DATETIMEOFFSET_MINDATE;
                    }
                    if (d.CreateDate < StorageAzureDataTablesConstants.DATETIMEOFFSET_MINDATE)
                        d.CreateDate = StorageAzureDataTablesConstants.DATETIMEOFFSET_MINDATE;

                    d.AuditType = s.AuditType;
                    //d.CreateDate ignore
                    d.Data = s.Data;
                    d.IPAddress = s.IPAddress;
                    d.RequestHeaders = s.RequestHeaders;
                    d.PartitionKey = d.UserId.ToString();
                    var reverseDate = DateTimeOffset.MaxValue.Ticks - d.CreateDate.Ticks;
                    d.RowKey =
                        reverseDate.ToString("d19") +
                        StorageAzureDataTablesConstants.KEY_DELIMITER +
                        d.Key.ToString();
                });
        }
    }
}