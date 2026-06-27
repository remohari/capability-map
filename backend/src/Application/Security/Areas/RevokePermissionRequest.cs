namespace CapabilityMap.Backend.Application.Security.Areas;

public sealed record RevokePermissionRequest(
    string UserId,
    string? ChangeReason = null);
