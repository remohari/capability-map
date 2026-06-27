namespace CapabilityMap.Backend.Domain.Security;

public sealed record AuthorizationDecision(
    string UserId,
    string? RoleName,
    string Target,
    AuthorizationResult Result,
    string ReasonCode,
    DateTimeOffset EvaluatedAt);

public enum AuthorizationResult
{
    Allowed,
    Denied
}
