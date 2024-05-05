using AutoMapper;


namespace ServiceBricks.Security.AzureDataTables
{
    /// <summary>
    /// This is an automapper profile for the ApplicationRole domain object.
    /// </summary>
    public class ApplicationRoleMappingProfile : Profile
    {
        public ApplicationRoleMappingProfile()
        {
            CreateMap<ApplicationRoleDto, ApplicationRole>()
                .ForMember(x => x.PartitionKey, y => y.MapFrom(z => z.StorageKey))
                .ForMember(x => x.Id, y => y.MapFrom(z => z.StorageKey))
                .ForMember(x => x.ETag, y => y.Ignore())
                .ForMember(x => x.Timestamp, y => y.Ignore())
                .ForMember(x => x.RowKey, y => y.Ignore());

            CreateMap<ApplicationRole, ApplicationRoleDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Id));
        }
    }
}