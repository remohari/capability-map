namespace CapabilityMap.Backend.Application.Security.Areas;

public sealed record ManageAreaPermissionRequest(
    string UserId,
    string Action,
    string? ChangeReason = null,
    DateTimeOffset? ExpiresAt = null);
