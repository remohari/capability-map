using CapabilityMap.Backend.Domain.Security;

namespace CapabilityMap.Backend.Security;

public sealed class CustomerAccessPolicy
{
    private readonly RoleAuthorizationService _authorizationService;

    public CustomerAccessPolicy(RoleAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    public AuthorizationDecision Evaluate(UserRoleContext user, string customerId)
    {
        return _authorizationService.EvaluateAccess(
            user,
            target: "customer-dashboard",
            requiredRole: RoleDefinitions.Customer,
            requiredCustomerScope: customerId);
    }
}
