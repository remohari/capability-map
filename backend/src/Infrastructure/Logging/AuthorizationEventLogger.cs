using CapabilityMap.Backend.Domain.Security;

namespace CapabilityMap.Backend.Infrastructure.Logging;

public sealed class AuthorizationEventLogger
{
    public AuthorizationAuditEvent CreateAccessDeniedEvent(
        AuthorizationDecision decision,
        string actorReference)
    {
        return new AuthorizationAuditEvent(
            EventType: "access_denied",
            ActorReference: actorReference,
            SubjectReference: Pseudonymize(decision.UserId),
            Target: decision.Target,
            Outcome: decision.Result.ToString().ToLowerInvariant(),
            ReasonCode: decision.ReasonCode,
            OccurredAt: decision.EvaluatedAt);
    }

    public AuthorizationAuditEvent CreateRoleMutationEvent(
        string eventType,
        string actorReference,
        string subjectUserId,
        string targetRole)
    {
        return new AuthorizationAuditEvent(
            EventType: eventType,
            ActorReference: actorReference,
            SubjectReference: Pseudonymize(subjectUserId),
            Target: targetRole,
            Outcome: "success",
            ReasonCode: "n/a",
            OccurredAt: DateTimeOffset.UtcNow);
    }

    private static string Pseudonymize(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "unknown";
        }

        return $"{value[..Math.Min(3, value.Length)]}***";
    }
}

public sealed record AuthorizationAuditEvent(
    string EventType,
    string ActorReference,
    string SubjectReference,
    string Target,
    string Outcome,
    string ReasonCode,
    DateTimeOffset OccurredAt);
