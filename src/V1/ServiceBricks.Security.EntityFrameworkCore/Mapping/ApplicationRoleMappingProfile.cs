using AutoMapper;


namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// This is an automapper profile for the ApplicationRole domain object.
    /// </summary>
    public class ApplicationRoleMappingProfile : Profile
    {
        public ApplicationRoleMappingProfile()
        {
            CreateMap<ApplicationRoleDto, ApplicationRole>()
                .ForMember(x => x.Id, y => y.MapFrom<KeyResolver>())
                .ForMember(x => x.ApplicationUserRoles, y => y.Ignore());

            CreateMap<ApplicationRole, ApplicationRoleDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Id));
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