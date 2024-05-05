using AutoMapper;

namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// This is an automapper profile for the ApplicationRoleClaim domain object.
    /// </summary>
    public class ApplicationRoleClaimMappingProfile : Profile
    {
        public ApplicationRoleClaimMappingProfile()
        {
            CreateMap<ApplicationRoleClaimDto, ApplicationRoleClaim>()
                .ForMember(x => x.Key, y => y.MapFrom<KeyResolver>())
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.RoleId, y => y.MapFrom<RoleIdResolver>());

            CreateMap<ApplicationRoleClaim, ApplicationRoleClaimDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Key))
                .ForMember(x => x.RoleStorageKey, y => y.MapFrom(z => z.RoleId));
        }

        public class KeyResolver : IValueResolver<DataTransferObject, object, Guid>
        {
            public Guid Resolve(DataTransferObject source, object destination, Guid sourceMember, ResolutionContext context)
            {
                if (string.IsNullOrEmpty(source.StorageKey))
                    return Guid.Empty;

                Guid tempKey;
                if (Guid.TryParse(source.StorageKey, out tempKey))
                    return tempKey;
                return Guid.Empty;
            }
        }

        public class RoleIdResolver : IValueResolver<ApplicationRoleClaimDto, object, Guid>
        {
            public Guid Resolve(ApplicationRoleClaimDto source, object destination, Guid sourceMember, ResolutionContext context)
            {
                if (string.IsNullOrEmpty(source.RoleStorageKey))
                    return Guid.Empty;

                Guid tempKey;
                if (Guid.TryParse(source.RoleStorageKey, out tempKey))
                    return tempKey;
                return Guid.Empty;
            }
        }
    }
}