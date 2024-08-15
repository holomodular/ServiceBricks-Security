using AutoMapper;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is an automapper profile for the ApplicationUser domain object.
    /// </summary>
    public partial class ApplicationUserMappingProfile : Profile
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ApplicationUserMappingProfile()
        {
            CreateMap<UserDto, ApplicationIdentityUser>()
                .ForMember(x => x.CreateDate, y => y.Ignore())
                .ForMember(x => x.Id, y => y.MapFrom(z => z.StorageKey));

            CreateMap<ApplicationIdentityUser, ApplicationUser>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.Id));

            CreateMap<UserDto, ApplicationUser>()
                .ForMember(x => x.CreateDate, y => y.Ignore())
                .ForMember(x => x.Id, y => y.MapFrom(z => z.StorageKey));

            CreateMap<ApplicationUser, ApplicationIdentityUser>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.Id));

            CreateMap<ApplicationIdentityUser, UserDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Id));

            CreateMap<ApplicationUser, UserDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Id));
        }
    }
}