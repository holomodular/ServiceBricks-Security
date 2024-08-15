using AutoMapper;

namespace ServiceBricks.Security.Cosmos
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
            CreateMap<RoleDto, ApplicationRole>()
                .ForMember(x => x.Id, y => y.MapFrom<KeyResolver>())
                .ForMember(x => x.ApplicationUserRoles, y => y.Ignore());

            CreateMap<ApplicationRole, RoleDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Id));
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
    }
}