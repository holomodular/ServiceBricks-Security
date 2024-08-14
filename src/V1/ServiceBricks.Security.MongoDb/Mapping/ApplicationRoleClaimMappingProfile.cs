using AutoMapper;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is an automapper profile for the ApplicationRoleClaim domain object.
    /// </summary>
    public partial class ApplicationRoleClaimMappingProfile : Profile
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ApplicationRoleClaimMappingProfile()
        {
            CreateMap<ApplicationRoleClaim, ApplicationIdentityRoleClaim>()
                .ForMember(x => x.Key, y => y.MapFrom(z => z.Key))
                .ForMember(x => x.RoleId, y => y.MapFrom(z => z.RoleId))
                .ForMember(x => x.Id, y => y.Ignore());

            CreateMap<ApplicationIdentityRoleClaim, ApplicationRoleClaimDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Id))
                .ForMember(x => x.RoleStorageKey, y => y.MapFrom(z => z.RoleId));

            CreateMap<ApplicationRoleClaim, ApplicationRoleClaimDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Key))
                .ForMember(x => x.RoleStorageKey, y => y.MapFrom(z => z.RoleId));

            CreateMap<ApplicationRoleClaimDto, ApplicationIdentityRoleClaim>()
                .ForMember(x => x.Key, y => y.MapFrom(z => z.StorageKey))
                .ForMember(x => x.RoleId, y => y.MapFrom(z => z.RoleStorageKey))
                .ForMember(x => x.Id, y => y.Ignore());

            CreateMap<ApplicationIdentityRoleClaim, ApplicationRoleClaim>()
                .ForMember(x => x.Key, y => y.MapFrom(z => z.Key))
                .ForMember(x => x.RoleId, y => y.MapFrom(z => z.RoleId));

            CreateMap<ApplicationRoleClaimDto, ApplicationRoleClaim>()
                .ForMember(x => x.Key, y => y.MapFrom(z => z.StorageKey))
                .ForMember(x => x.RoleId, y => y.MapFrom(z => z.RoleStorageKey));
        }
    }
}