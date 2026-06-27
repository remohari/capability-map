using CapabilityMap.Backend.Domain.Security;

namespace CapabilityMap.Backend.Infrastructure.Logging;

public sealed class AreaEventLogger
{
    private readonly AuthorizationEventLogger _inner;

    public AreaEventLogger(AuthorizationEventLogger inner)
    {
        _inner = inner;
    }

    public AuthorizationAuditEvent CreateAreaCreatedEvent(string actorReference, string subjectUserId, string targetArea)
    {
        return _inner.CreateRoleMutationEvent("area_created", actorReference, subjectUserId, targetArea);
    }

    public AuthorizationAuditEvent CreatePermissionGrantedEvent(string actorReference, string subjectUserId, string targetArea)
    {
        return _inner.CreateRoleMutationEvent("permission_granted", actorReference, subjectUserId, targetArea);
    }

    public AuthorizationAuditEvent CreatePermissionRevokedEvent(string actorReference, string subjectUserId, string targetArea)
    {
        return _inner.CreateRoleMutationEvent("permission_revoked", actorReference, subjectUserId, targetArea);
    }
}
