using AutoMapper;

using ServiceBricks.Storage.MongoDb;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is an automapper profile for the ApplicationUserClaim domain object.
    /// </summary>
    public class ApplicationUserClaimMappingProfile : Profile
    {
        public ApplicationUserClaimMappingProfile()
        {
            CreateMap<ApplicationUserClaimDto, ApplicationIdentityUserClaim>()
                .ForMember(x => x.Key, y => y.MapFrom(z => z.StorageKey))
                .ForMember(x => x.UserId, y => y.MapFrom(z => z.UserStorageKey))
                .ForMember(x => x.Id, y => y.Ignore());

            CreateMap<ApplicationIdentityUserClaim, ApplicationUserClaim>()
                .ForMember(x => x.Key, y => y.MapFrom(z => z.Key))
                .ForMember(x => x.UserId, y => y.MapFrom(z => z.UserId));

            CreateMap<ApplicationUserClaimDto, ApplicationUserClaim>()
                .ForMember(x => x.Key, y => y.MapFrom(z => z.StorageKey))
                .ForMember(x => x.UserId, y => y.MapFrom(z => z.UserStorageKey));

            CreateMap<ApplicationUserClaim, ApplicationIdentityUserClaim>()
                .ForMember(x => x.Key, y => y.MapFrom(z => z.Key))
                .ForMember(x => x.UserId, y => y.MapFrom(z => z.UserId))
                .ForMember(x => x.Id, y => y.Ignore());

            CreateMap<ApplicationIdentityUserClaim, ApplicationUserClaimDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Key))
                .ForMember(x => x.UserStorageKey, y => y.MapFrom(z => z.UserId));

            CreateMap<ApplicationUserClaim, ApplicationUserClaimDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Key))
                .ForMember(x => x.UserStorageKey, y => y.MapFrom(z => z.UserId));
        }
    }
}