namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// This is an mapper profile for the ApplicationRole domain object.
    /// </summary>
    public partial class ApplicationRoleMappingProfile
    {
        /// <summary>
        /// Register the mapping
        /// </summary>
        public static void Register(IMapperRegistry registry)
        {
            registry.Register<ApplicationRole, RoleDto>(
                (s, d) =>
                {
                    d.ConcurrencyStamp = s.ConcurrencyStamp;
                    d.Name = s.Name;
                    d.NormalizedName = s.NormalizedName;
                    d.StorageKey = s.Id.ToString();
                });

            registry.Register<RoleDto, ApplicationRole>(
                (s, d) =>
                {
                    d.ConcurrencyStamp = s.ConcurrencyStamp;
                    d.Name = s.Name;
                    d.NormalizedName = s.NormalizedName;
                    Guid tempId;
                    if (Guid.TryParse(s.StorageKey, out tempId))
                        d.Id = tempId;
                });
        }
    }
}