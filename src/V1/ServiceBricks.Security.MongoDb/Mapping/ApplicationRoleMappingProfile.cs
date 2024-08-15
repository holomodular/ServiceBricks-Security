using AutoMapper;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is an automapper profile for the ApplicationRole domain object.
    /// </summary>
    public partial class ApplicationRoleMappingProfile : Profile
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ApplicationRoleMappingProfile()
        {
            CreateMap<RoleDto, ApplicationIdentityRole>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.StorageKey));

            CreateMap<ApplicationIdentityRole, ApplicationRole>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.Id));

            CreateMap<RoleDto, ApplicationRole>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.StorageKey));

            CreateMap<ApplicationRole, ApplicationIdentityRole>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.Id));

            CreateMap<ApplicationIdentityRole, RoleDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Id));

            CreateMap<ApplicationRole, RoleDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Id));
        }
    }
}