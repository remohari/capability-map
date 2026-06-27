using CapabilityMap.Backend.Domain.Security;
using CapabilityMap.Backend.Infrastructure.Logging;

namespace CapabilityMap.Backend.Tests.Integration.Security;

public sealed class AuthorizationAuditLoggingTests
{
    [Fact]
    public void AccessDenied_ShouldEmitStructuredAuditEvent()
    {
        var logger = new AuthorizationEventLogger();
        var decision = new AuthorizationDecision(
            "customer-user",
            "Kund:in",
            "admin-dashboard",
            AuthorizationResult.Denied,
            "wrong_role",
            DateTimeOffset.UtcNow);

        var result = logger.CreateAccessDeniedEvent(decision, "customer@example.invalid");

        Assert.Equal("access_denied", result.EventType);
        Assert.Equal("wrong_role", result.ReasonCode);
        Assert.Equal("customer@example.invalid", result.ActorReference);
    }

    [Fact]
    public void RoleChange_ShouldEmitStructuredAuditEvent()
    {
        var logger = new AuthorizationEventLogger();

        var result = logger.CreateRoleMutationEvent("role_changed", "admin@example.invalid", "user-1", "Berater:in");

        Assert.Equal("role_changed", result.EventType);
        Assert.Equal("success", result.Outcome);
        Assert.Equal("Berater:in", result.Target);
    }
}
