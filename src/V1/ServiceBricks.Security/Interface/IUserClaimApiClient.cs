﻿namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a REST API client for the ApplicationUserClaim domain object.
    /// </summary>
    public partial interface IUserClaimApiClient : IApiClient<UserClaimDto>, IUserClaimApiService
    {
    }
}