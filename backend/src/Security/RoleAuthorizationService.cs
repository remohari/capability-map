using CapabilityMap.Backend.Domain.Security;

namespace CapabilityMap.Backend.Security;

public sealed class RoleAuthorizationService
{
    private readonly ISystemClock _clock;

    public RoleAuthorizationService(ISystemClock? clock = null)
    {
        _clock = clock ?? new SystemClock();
    }

    public AuthorizationDecision EvaluateAccess(
        UserRoleContext user,
        string target,
        string? requiredRole = null,
        string? requiredCustomerScope = null)
    {
        if (user.Status != UserAccountStatus.Active)
        {
            return Denied(user, target, "inactive_user");
        }

        if (string.IsNullOrWhiteSpace(user.ActiveRole))
        {
            return Denied(user, target, "missing_role");
        }

        if (!RoleDefinitions.RightsMatrix.TryGetValue(user.ActiveRole, out var capabilities))
        {
            return Denied(user, target, "unknown_role");
        }

        if (requiredRole is not null && !string.Equals(user.ActiveRole, requiredRole, StringComparison.Ordinal))
        {
            return Denied(user, target, "wrong_role");
        }

        if (!capabilities.VisibleAreas.Contains(target, StringComparer.Ordinal) &&
            !capabilities.AllowedActions.Contains(target, StringComparer.Ordinal))
        {
            return Denied(user, target, "target_not_allowed");
        }

        if (requiredCustomerScope is not null &&
            !string.Equals(user.CustomerScope, requiredCustomerScope, StringComparison.Ordinal))
        {
            return Denied(user, target, "wrong_scope");
        }

        return new AuthorizationDecision(
            user.UserId,
            user.ActiveRole,
            target,
            AuthorizationResult.Allowed,
            "allowed",
            _clock.UtcNow);
    }

    private AuthorizationDecision Denied(UserRoleContext user, string target, string reasonCode)
    {
        return new AuthorizationDecision(
            user.UserId,
            user.ActiveRole,
            target,
            AuthorizationResult.Denied,
            reasonCode,
            _clock.UtcNow);
    }
}

public interface ISystemClock
{
    DateTimeOffset UtcNow { get; }
}

public sealed class SystemClock : ISystemClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
