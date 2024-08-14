﻿using AutoMapper;

namespace ServiceBricks.Security.EntityFrameworkCore
{
    /// <summary>
    /// This is a API service for the ApplicationUserClaim domain object.
    /// </summary>
    public partial class ApplicationUserClaimApiService : ApiService<ApplicationUserClaim, ApplicationUserClaimDto>, IApplicationUserClaimApiService
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="businessRuleService"></param>
        /// <param name="repository"></param>
        public ApplicationUserClaimApiService(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IDomainRepository<ApplicationUserClaim> repository) : base(mapper, businessRuleService, repository)
        {
        }
    }
}