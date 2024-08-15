using AutoMapper;

namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// This is an automapper profile for the ApplicationUserLogin domain object.
    /// </summary>
    public partial class ApplicationUserLoginMappingProfile : Profile
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ApplicationUserLoginMappingProfile()
        {
            CreateMap<UserLoginDto, ApplicationUserLogin>()
                .ForMember(x => x.UserId, y => y.MapFrom<UserIdResolver>())
                .ForMember(x => x.Key, y => y.MapFrom<KeyResolver>());

            CreateMap<ApplicationUserLogin, UserLoginDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Key))
                .ForMember(x => x.UserStorageKey, y => y.MapFrom(z => z.UserId));
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
        /// Resolve the user id from the user storage key.
        /// </summary>
        public class UserIdResolver : IValueResolver<UserLoginDto, object, Guid>
        {
            /// <summary>
            /// Resolve the user id from the user storage key.
            /// </summary>
            /// <param name="source"></param>
            /// <param name="destination"></param>
            /// <param name="sourceMember"></param>
            /// <param name="context"></param>
            /// <returns></returns>
            public Guid Resolve(UserLoginDto source, object destination, Guid sourceMember, ResolutionContext context)
            {
                if (string.IsNullOrEmpty(source.UserStorageKey))
                    return Guid.Empty;

                Guid tempKey;
                if (Guid.TryParse(source.UserStorageKey, out tempKey))
                    return tempKey;
                return Guid.Empty;
            }
        }
    }
}