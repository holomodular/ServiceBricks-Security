using AutoMapper;

namespace ServiceBricks.Security.MongoDb
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
                .ForMember(x => x.Key, y => y.MapFrom(z => z.StorageKey))
                .ForMember(x => x.UserId, y => y.MapFrom(z => z.UserStorageKey));

            CreateMap<UserAudit, UserAuditDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Key))
                .ForMember(x => x.UserStorageKey, y => y.MapFrom(z => z.UserId));
        }
    }
}