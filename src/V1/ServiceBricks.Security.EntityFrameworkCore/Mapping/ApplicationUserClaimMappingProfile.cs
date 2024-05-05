﻿using AutoMapper;


namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// This is an automapper profile for the ApplicationUserClaim domain object.
    /// </summary>
    public class ApplicationUserClaimMappingProfile : Profile
    {
        public ApplicationUserClaimMappingProfile()
        {
            CreateMap<ApplicationUserClaimDto, ApplicationUserClaim>()
                .ForMember(x => x.Id, y => y.MapFrom<KeyResolver>())
                .ForMember(x => x.UserId, y => y.MapFrom<UserIdResolver>());

            CreateMap<ApplicationUserClaim, ApplicationUserClaimDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Id))
                .ForMember(x => x.UserStorageKey, y => y.MapFrom(z => z.UserId));
        }

        public class KeyResolver : IValueResolver<DataTransferObject, object, int>
        {
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

        public class UserIdResolver : IValueResolver<ApplicationUserClaimDto, object, Guid>
        {
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