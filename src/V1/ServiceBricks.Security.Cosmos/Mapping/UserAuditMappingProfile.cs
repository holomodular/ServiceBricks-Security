using AutoMapper;

namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// This is an automapper profile for the AuditUser domain object.
    /// </summary>
    public partial class UserAuditMappingProfile : Profile
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public UserAuditMappingProfile()
        {
            CreateMap<UserAuditDto, UserAudit>()
                .ForMember(x => x.CreateDate, y => y.Ignore())
                .ForMember(x => x.Key, y => y.MapFrom<KeyResolver>())
                .ForMember(x => x.UserId, y => y.MapFrom(z => z.UserStorageKey));

            CreateMap<UserAudit, UserAuditDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Key))
                .ForMember(x => x.UserStorageKey, y => y.MapFrom(z => z.UserId));
        }

        /// <summary>
        /// Resolves the key from the storage key.
        /// </summary>
        public class KeyResolver : IValueResolver<DataTransferObject, object, Guid>
        {
            /// <summary>
            /// Resolves the key from the storage key.
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