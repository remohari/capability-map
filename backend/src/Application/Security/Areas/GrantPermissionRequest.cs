namespace CapabilityMap.Backend.Application.Security.Areas;

public sealed record GrantPermissionRequest(
    string UserId,
    string? ChangeReason = null,
    DateTimeOffset? ExpiresAt = null);
