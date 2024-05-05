using AutoMapper;

using ServiceBricks.Storage.AzureDataTables;

namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is an automapper profile for the ApplicationUserLogin domain object.
    /// </summary>
    public class ApplicationUserLoginMappingProfile : Profile
    {
        public ApplicationUserLoginMappingProfile()
        {
            CreateMap<ApplicationUserLoginDto, ApplicationUserLogin>()
                .ForMember(x => x.PartitionKey, y => y.MapFrom<PartitionKeyResolver>())
                .ForMember(x => x.RowKey, y => y.MapFrom<RowKeyResolver>())
                .ForMember(x => x.UserId, y => y.MapFrom(z => z.UserStorageKey))
                .ForMember(x => x.ETag, y => y.Ignore())
                .ForMember(x => x.Timestamp, y => y.Ignore());

            CreateMap<ApplicationUserLogin, ApplicationUserLoginDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom<StorageKeyResolver>())
                .ForMember(x => x.UserStorageKey, y => y.MapFrom(z => z.UserId));
        }
    }
}