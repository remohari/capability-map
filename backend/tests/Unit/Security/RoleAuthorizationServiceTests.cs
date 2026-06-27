using CapabilityMap.Backend.Domain.Security;
using CapabilityMap.Backend.Security;

namespace CapabilityMap.Backend.Tests.Unit.Security;

public sealed class RoleAuthorizationServiceTests
{
    private readonly RoleAuthorizationService _service = new(new FixedClock());

    [Fact]
    public void EvaluateAccess_ShouldDeny_WhenUserHasNoActiveRole()
    {
        var decision = _service.EvaluateAccess(
            new UserRoleContext("user-1", "user-1", null, UserAccountStatus.Active, "customer-001"),
            target: "customer-dashboard",
            requiredRole: RoleDefinitions.Customer,
            requiredCustomerScope: "customer-001");

        Assert.Equal(AuthorizationResult.Denied, decision.Result);
        Assert.Equal("missing_role", decision.ReasonCode);
    }

    [Fact]
    public void EvaluateAccess_ShouldDeny_WhenRoleDoesNotMatchRequiredRole()
    {
        var decision = _service.EvaluateAccess(
            new UserRoleContext("user-1", "user-1", RoleDefinitions.Customer, UserAccountStatus.Active, "customer-001"),
            target: "customer-dashboard",
            requiredRole: RoleDefinitions.Admin);

        Assert.Equal(AuthorizationResult.Denied, decision.Result);
        Assert.Equal("wrong_role", decision.ReasonCode);
    }

    [Fact]
    public void EvaluateAccess_ShouldAllow_WhenTargetBelongsToAssignedRole()
    {
        var decision = _service.EvaluateAccess(
            new UserRoleContext("user-1", "user-1", RoleDefinitions.Advisor, UserAccountStatus.Active),
            target: "advisor-cases",
            requiredRole: RoleDefinitions.Advisor);

        Assert.Equal(AuthorizationResult.Allowed, decision.Result);
        Assert.Equal("allowed", decision.ReasonCode);
    }

    [Fact]
    public void EvaluateAccess_ShouldDeny_WhenCustomerScopeDoesNotMatch()
    {
        var decision = _service.EvaluateAccess(
            new UserRoleContext("user-1", "user-1", RoleDefinitions.Customer, UserAccountStatus.Active, "customer-001"),
            target: "customer-dashboard",
            requiredRole: RoleDefinitions.Customer,
            requiredCustomerScope: "customer-999");

        Assert.Equal(AuthorizationResult.Denied, decision.Result);
        Assert.Equal("wrong_scope", decision.ReasonCode);
    }

    private sealed class FixedClock : ISystemClock
    {
        public DateTimeOffset UtcNow => new(2026, 06, 27, 12, 00, 00, TimeSpan.Zero);
    }
}
