using AutoMapper;

namespace ServiceBricks.Security.Cosmos
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
            CreateMap<RoleClaimDto, ApplicationRoleClaim>()
                .ForMember(x => x.Key, y => y.MapFrom<KeyResolver>())
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.RoleId, y => y.MapFrom<RoleIdResolver>());

            CreateMap<ApplicationRoleClaim, RoleClaimDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Key))
                .ForMember(x => x.RoleStorageKey, y => y.MapFrom(z => z.RoleId));
        }

        /// <summary>
        /// Resolve the key from the storage key.
        /// </summary>
        public class KeyResolver : IValueResolver<DataTransferObject, object, Guid>
        {
            /// <summary>
            /// Resolve the key from the storage key.
            /// </summary>
            /// <param name="source"></param>
            /// <param name="destination"></param>
            /// <param name="sourceMember"></param>
            /// <param name="context"></param>
            /// <returns></returns>
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

        /// <summary>
        /// Resolve the role id from the role storage key.
        /// </summary>
        public class RoleIdResolver : IValueResolver<RoleClaimDto, object, Guid>
        {
            /// <summary>
            /// Resolve the role id from the role storage key.
            /// </summary>
            /// <param name="source"></param>
            /// <param name="destination"></param>
            /// <param name="sourceMember"></param>
            /// <param name="context"></param>
            /// <returns></returns>
            public Guid Resolve(RoleClaimDto source, object destination, Guid sourceMember, ResolutionContext context)
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