namespace CapabilityMap.Backend.Domain.Security;

public sealed record RoleAssignment(
    Guid AssignmentId,
    string UserId,
    string RoleName,
    string AssignedBy,
    DateTimeOffset AssignedAt,
    string? ChangeReason = null);
