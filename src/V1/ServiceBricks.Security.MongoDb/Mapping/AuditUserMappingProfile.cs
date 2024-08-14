using AutoMapper;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is an automapper profile for the AuditUser domain object.
    /// </summary>
    public partial class AuditUserMappingProfile : Profile
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public AuditUserMappingProfile()
        {
            CreateMap<AuditUserDto, AuditUser>()
                .ForMember(x => x.CreateDate, y => y.Ignore())
                .ForMember(x => x.Key, y => y.MapFrom(z => z.StorageKey))
                .ForMember(x => x.UserId, y => y.MapFrom(z => z.UserStorageKey));

            CreateMap<AuditUser, AuditUserDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Key))
                .ForMember(x => x.UserStorageKey, y => y.MapFrom(z => z.UserId));
        }
    }
}