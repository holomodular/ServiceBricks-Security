using AutoMapper;


namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is an automapper profile for the ApplicationUser domain object.
    /// </summary>
    public class ApplicationUserMappingProfile : Profile
    {
        public ApplicationUserMappingProfile()
        {
            CreateMap<ApplicationUserDto, ApplicationUser>()
                .ForMember(x => x.CreateDate, y => y.Ignore())
                .ForMember(x => x.PartitionKey, y => y.MapFrom(z => z.StorageKey))
                .ForMember(x => x.Id, y => y.MapFrom(z => z.StorageKey))
                .ForMember(x => x.RowKey, y => y.Ignore())
                .ForMember(x => x.ETag, y => y.Ignore())
                .ForMember(x => x.Timestamp, y => y.Ignore());

            CreateMap<ApplicationUser, ApplicationUserDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Id));
        }
    }
}