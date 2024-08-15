using AutoMapper;

using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is an automapper profile for the ApplicationUserRole domain object.
    /// </summary>
    public partial class ApplicationUserRoleMappingProfile : Profile
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ApplicationUserRoleMappingProfile()
        {
            CreateMap<UserRoleDto, ApplicationUserRole>()
                .ForMember(x => x.PartitionKey, y => y.MapFrom<PartitionKeyResolver>())
                .ForMember(x => x.RowKey, y => y.MapFrom<RowKeyResolver>())
                .ForMember(x => x.UserId, y => y.MapFrom(z => z.UserStorageKey))
                .ForMember(x => x.RoleId, y => y.MapFrom(z => z.RoleStorageKey))
                .ForMember(x => x.ETag, y => y.Ignore())
                .ForMember(x => x.Timestamp, y => y.Ignore());

            CreateMap<ApplicationUserRole, UserRoleDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom<StorageKeyResolver>())
                .ForMember(x => x.UserStorageKey, y => y.MapFrom(z => z.UserId))
                .ForMember(x => x.RoleStorageKey, y => y.MapFrom(z => z.RoleId));
        }
    }
}