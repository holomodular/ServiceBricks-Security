using AutoMapper;

using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is an automapper profile for the ApplicationRoleClaim domain object.
    /// </summary>
    public class ApplicationRoleClaimMappingProfile : Profile
    {
        public ApplicationRoleClaimMappingProfile()
        {
            CreateMap<ApplicationRoleClaimDto, ApplicationRoleClaim>()
                .ForMember(x => x.PartitionKey, y => y.MapFrom<PartitionKeyResolver>())
                .ForMember(x => x.RowKey, y => y.MapFrom<RowKeyResolver>())
                .ForMember(x => x.RoleId, y => y.MapFrom(z => z.RoleStorageKey))
                .ForMember(x => x.ETag, y => y.Ignore())
                .ForMember(x => x.Timestamp, y => y.Ignore())
                .ForMember(x => x.Key, y => y.Ignore())
                .ForMember(x => x.Id, y => y.Ignore());

            CreateMap<ApplicationRoleClaim, ApplicationRoleClaimDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom<StorageKeyResolver>())
                .ForMember(x => x.RoleStorageKey, y => y.MapFrom(z => z.RoleId));
        }
    }
}