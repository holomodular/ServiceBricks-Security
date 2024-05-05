using AutoMapper;


namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is an automapper profile for the ApplicationUser domain object.
    /// </summary>
    public class ApplicationUserMappingProfile : Profile
    {
        public ApplicationUserMappingProfile()
        {
            CreateMap<ApplicationUserDto, ApplicationIdentityUser>()
                .ForMember(x => x.CreateDate, y => y.Ignore())
                .ForMember(x => x.Id, y => y.MapFrom(z => z.StorageKey));

            CreateMap<ApplicationIdentityUser, ApplicationUser>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.Id));

            CreateMap<ApplicationUserDto, ApplicationUser>()
                .ForMember(x => x.CreateDate, y => y.Ignore())
                .ForMember(x => x.Id, y => y.MapFrom(z => z.StorageKey));

            CreateMap<ApplicationUser, ApplicationIdentityUser>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.Id));

            CreateMap<ApplicationIdentityUser, ApplicationUserDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Id));

            CreateMap<ApplicationUser, ApplicationUserDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Id));
        }
    }
}