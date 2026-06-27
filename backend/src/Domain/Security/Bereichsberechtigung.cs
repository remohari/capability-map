namespace CapabilityMap.Backend.Domain.Security;

public sealed record Bereichsberechtigung(
    Guid PermissionId,
    Guid AreaId,
    string UserId,
    string Status,
    DateTimeOffset GrantedAt,
    string ChangedBy,
    string? ChangeReason,
    DateTimeOffset? ExpiresAt,
    DateTimeOffset? ChangedAt = null);
