﻿namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a REST API client for the ApplicationRole domain object.
    /// </summary>
    public partial interface IRoleApiClient : IApiClient<RoleDto>, IRoleApiService
    {
    }
}