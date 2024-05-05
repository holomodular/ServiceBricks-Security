using Microsoft.AspNetCore.Mvc;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a REST API controller for the AuditUser domain object.
    /// </summary>
    public interface IAuditUserApiController : IApiController<AuditUserDto>
    {
    }
}