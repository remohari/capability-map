using CapabilityMap.Backend.Domain.Security;
using CapabilityMap.Backend.Infrastructure.Logging;
using CapabilityMap.Backend.Infrastructure.Persistence;
using CapabilityMap.Backend.Security;

namespace CapabilityMap.Backend.Application.Security.RoleAssignments;

public sealed class RoleAssignmentService
{
    private readonly IRoleAssignmentStore _store;
    private readonly AuthorizationEventLogger _auditLogger;
    private readonly List<AuthorizationAuditEvent> _auditTrail = [];

    public RoleAssignmentService(
        IRoleAssignmentStore store,
        AuthorizationEventLogger auditLogger)
    {
        _store = store;
        _auditLogger = auditLogger;
    }

    public IReadOnlyCollection<RoleAssignmentResponse> GetAll()
    {
        return _store.GetAll().Select(ToResponse).ToArray();
    }

    public RoleAssignmentResponse? Get(string userId)
    {
        return _store.Get(userId) is { } assignment ? ToResponse(assignment) : null;
    }

    public RoleAssignmentResponse Assign(
        string targetUserId,
        string actorReference,
        RoleAssignmentRequest request)
    {
        ValidateRole(request.RoleName);

        var existing = _store.Get(targetUserId);
        var assignment = new RoleAssignment(
            existing?.AssignmentId ?? Guid.NewGuid(),
            targetUserId,
            request.RoleName,
            actorReference,
            DateTimeOffset.UtcNow,
            request.ChangeReason);

        _store.Upsert(assignment);

        var eventType = existing is null ? "role_assigned" : "role_changed";
        _auditTrail.Add(_auditLogger.CreateRoleMutationEvent(eventType, actorReference, targetUserId, request.RoleName));

        return ToResponse(assignment);
    }

    public bool Remove(string targetUserId, string actorReference)
    {
        var removed = _store.Remove(targetUserId);
        if (removed is null)
        {
            return false;
        }

        _auditTrail.Add(_auditLogger.CreateRoleMutationEvent("role_removed", actorReference, targetUserId, removed.RoleName));
        return true;
    }

    public IReadOnlyCollection<AuthorizationAuditEvent> GetAuditTrail()
    {
        return _auditTrail.AsReadOnly();
    }

    private static void ValidateRole(string roleName)
    {
        if (!RoleDefinitions.RightsMatrix.ContainsKey(roleName))
        {
            throw new InvalidOperationException($"Unknown role '{roleName}'.");
        }
    }

    private static RoleAssignmentResponse ToResponse(RoleAssignment assignment)
    {
        return new RoleAssignmentResponse(
            assignment.AssignmentId,
            assignment.UserId,
            assignment.RoleName,
            assignment.AssignedBy,
            assignment.AssignedAt,
            assignment.ChangeReason);
    }
}
