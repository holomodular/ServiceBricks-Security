using AutoMapper;


namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is an automapper profile for the ApplicationRole domain object.
    /// </summary>
    public class ApplicationRoleMappingProfile : Profile
    {
        public ApplicationRoleMappingProfile()
        {
            CreateMap<ApplicationRoleDto, ApplicationIdentityRole>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.StorageKey));

            CreateMap<ApplicationIdentityRole, ApplicationRole>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.Id));

            CreateMap<ApplicationRoleDto, ApplicationRole>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.StorageKey));

            CreateMap<ApplicationRole, ApplicationIdentityRole>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.Id));

            CreateMap<ApplicationIdentityRole, ApplicationRoleDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Id));

            CreateMap<ApplicationRole, ApplicationRoleDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Id));
        }
    }
}