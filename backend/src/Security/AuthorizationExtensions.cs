namespace CapabilityMap.Backend.Security;

public static class AuthorizationExtensions
{
    public static IReadOnlyList<string> PlannedPolicies =>
    [
        "AdminOnly",
        "CustomerOnly",
        "AdvisorOnly",
        "EvaluatorOnly",
        "AssignedCustomerScope"
    ];
}
