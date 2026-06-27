using CapabilityMap.Backend.Domain.Security;
using CapabilityMap.Backend.Security;

namespace CapabilityMap.Backend.Api.Controllers;

public static class RequestUserContextFactory
{
    public static UserRoleContext Create(HttpRequest request)
    {
        var userId = request.Headers["X-User-Id"].FirstOrDefault() ?? "anonymous";
        var displayReference = request.Headers["X-Display-Reference"].FirstOrDefault() ?? userId;
        var activeRole = request.Headers["X-Active-Role"].FirstOrDefault();
        var customerScope = request.Headers["X-Customer-Scope"].FirstOrDefault();
        var statusHeader = request.Headers["X-User-Status"].FirstOrDefault();

        var status = statusHeader?.ToLowerInvariant() switch
        {
            "inactive" => UserAccountStatus.Inactive,
            "suspended" => UserAccountStatus.Suspended,
            _ => UserAccountStatus.Active
        };

        return new UserRoleContext(
            userId,
            displayReference,
            activeRole,
            status,
            customerScope);
    }
}
