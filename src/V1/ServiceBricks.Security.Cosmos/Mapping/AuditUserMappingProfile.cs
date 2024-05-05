using AutoMapper;

namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// This is an automapper profile for the AuditUser domain object.
    /// </summary>
    public class AuditUserMappingProfile : Profile
    {
        public AuditUserMappingProfile()
        {
            CreateMap<AuditUserDto, AuditUser>()
                .ForMember(x => x.CreateDate, y => y.Ignore())
                .ForMember(x => x.Key, y => y.MapFrom<KeyResolver>())
                .ForMember(x => x.UserId, y => y.MapFrom(z => z.UserStorageKey));

            CreateMap<AuditUser, AuditUserDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Key))
                .ForMember(x => x.UserStorageKey, y => y.MapFrom(z => z.UserId));
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
    }
}