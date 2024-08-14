using AutoMapper;

namespace ServiceBricks.Security.MongoDb
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
            CreateMap<ApplicationUserRoleDto, ApplicationUserRole>()
                .ForMember(x => x.Key, y => y.MapFrom(z => z.StorageKey))
                .ForMember(x => x.UserId, y => y.MapFrom(z => z.UserStorageKey))
                .ForMember(x => x.RoleId, y => y.MapFrom(z => z.RoleStorageKey));

            CreateMap<ApplicationUserRole, ApplicationUserRoleDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Key))
                .ForMember(x => x.UserStorageKey, y => y.MapFrom(z => z.UserId))
                .ForMember(x => x.RoleStorageKey, y => y.MapFrom(z => z.RoleId));
        }
    }
}