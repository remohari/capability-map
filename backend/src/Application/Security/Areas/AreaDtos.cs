namespace CapabilityMap.Backend.Application.Security.Areas;

public sealed record AreaResponse(
    Guid AreaId,
    Guid CustomerId,
    string CustomerName,
    string Name,
    string? Description,
    string Status);

public sealed record AreaPermissionResponse(
    Guid PermissionId,
    Guid AreaId,
    string UserId,
    string Action,
    DateTimeOffset GrantedAt,
    string GrantedBy,
    string? ChangeReason,
    DateTimeOffset? ExpiresAt);
