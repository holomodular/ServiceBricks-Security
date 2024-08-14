using AutoMapper;

namespace ServiceBricks.Security.EntityFrameworkCore
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
            CreateMap<ApplicationRoleClaimDto, ApplicationRoleClaim>()
                .ForMember(x => x.Id, y => y.MapFrom<KeyResolver>())
                .ForMember(x => x.RoleId, y => y.MapFrom<RoleIdResolver>());

            CreateMap<ApplicationRoleClaim, ApplicationRoleClaimDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Id))
                .ForMember(x => x.RoleStorageKey, y => y.MapFrom(z => z.RoleId));
        }

        /// <summary>
        /// Resolve the key.
        /// </summary>
        public class KeyResolver : IValueResolver<DataTransferObject, object, int>
        {
            /// <summary>
            /// Resolve the key.
            /// </summary>
            /// <param name="source"></param>
            /// <param name="destination"></param>
            /// <param name="sourceMember"></param>
            /// <param name="context"></param>
            /// <returns></returns>
            public int Resolve(DataTransferObject source, object destination, int sourceMember, ResolutionContext context)
            {
                if (string.IsNullOrEmpty(source.StorageKey))
                    return 0;

                int tempKey;
                if (int.TryParse(source.StorageKey, out tempKey))
                    return tempKey;
                return 0;
            }
        }

        /// <summary>
        /// Resolve the role id.
        /// </summary>
        public class RoleIdResolver : IValueResolver<ApplicationRoleClaimDto, object, Guid>
        {
            /// <summary>
            /// Resolve the role id.
            /// </summary>
            /// <param name="source"></param>
            /// <param name="destination"></param>
            /// <param name="sourceMember"></param>
            /// <param name="context"></param>
            /// <returns></returns>
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