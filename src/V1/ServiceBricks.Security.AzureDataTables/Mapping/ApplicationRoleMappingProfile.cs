﻿using AutoMapper;

namespace ServiceBricks.Security.AzureDataTables
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
                .ForMember(x => x.PartitionKey, y => y.MapFrom(z => z.StorageKey))
                .ForMember(x => x.Id, y => y.MapFrom(z => z.StorageKey))
                .ForMember(x => x.ETag, y => y.Ignore())
                .ForMember(x => x.Timestamp, y => y.Ignore())
                .ForMember(x => x.RowKey, y => y.Ignore());

            CreateMap<ApplicationRole, RoleDto>()
                .ForMember(x => x.StorageKey, y => y.MapFrom(z => z.Id));
        }
    }
}