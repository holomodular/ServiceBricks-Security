using AutoMapper;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// This is an automapper profile for the ApplicationUserClaim domain object.
    /// </summary>
    public partial class ApplicationUserClaimMappingProfile : Profile
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ApplicationUserClaimMappingProfile()
        {
            CreateMap<ApplicationUserClaimDto, ApplicationUserClaim>()
                .ForMember(x => x.Id, y => y.MapFrom<KeyResolver>())
                .ForMember(x => x.UserId, y => y.MapFrom<UserIdResolver>());

            CreateMap<ApplicationUserClaim, ApplicationUserClaimDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Id))
                .ForMember(x => x.UserStorageKey, y => y.MapFrom(z => z.UserId));
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
        /// Resolve the user id.
        /// </summary>
        public class UserIdResolver : IValueResolver<ApplicationUserClaimDto, object, Guid>
        {
            /// <summary>
            /// Resolve the user id.
            /// </summary>
            /// <param name="source"></param>
            /// <param name="destination"></param>
            /// <param name="sourceMember"></param>
            /// <param name="context"></param>
            /// <returns></returns>
            public Guid Resolve(ApplicationUserClaimDto source, object destination, Guid sourceMember, ResolutionContext context)
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