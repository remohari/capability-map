namespace CapabilityMap.Backend.Application.Security.RoleAssignments;

public sealed record RoleAssignmentRequest(
    string RoleName,
    string? ChangeReason = null);

public sealed record RoleAssignmentResponse(
    Guid AssignmentId,
    string UserId,
    string RoleName,
    string AssignedBy,
    DateTimeOffset AssignedAt,
    string? ChangeReason);
