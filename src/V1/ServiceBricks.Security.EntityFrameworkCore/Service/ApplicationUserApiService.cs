﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;

using ServiceQuery;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// This is a API service for the ApplicationUser domain object.
    /// </summary>
    public class ApplicationUserApiService : ApiService<ApplicationUser, ApplicationUserDto>, IApplicationUserApiService
    {
        public ApplicationUserApiService(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IDomainRepository<ApplicationUser> repositoryApplicationUser)
            : base(mapper, businessRuleService, repositoryApplicationUser)
        {
        }
    }
}