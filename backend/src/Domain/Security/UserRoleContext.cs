namespace CapabilityMap.Backend.Domain.Security;

public sealed record UserRoleContext(
    string UserId,
    string DisplayReference,
    string? ActiveRole,
    UserAccountStatus Status,
    string? CustomerScope = null);

public enum UserAccountStatus
{
    Active,
    Inactive,
    Suspended
}
