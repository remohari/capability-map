namespace CapabilityMap.Backend.Security;

public static class RoleDefinitions
{
    public const string Customer = "Kund:in";
    public const string Advisor = "Berater:in";
    public const string Evaluator = "Bewerter:in";
    public const string Admin = "Admin";

    public static readonly IReadOnlyDictionary<string, RoleCapabilitySet> RightsMatrix =
        new Dictionary<string, RoleCapabilitySet>
        {
            [Customer] = new(
                VisibleAreas: ["customer-dashboard", "customer-profile"],
                AllowedActions: ["view-own-content", "use-own-functions"]),
            [Advisor] = new(
                VisibleAreas: ["advisor-dashboard", "advisor-cases"],
                AllowedActions: ["view-assigned-cases", "edit-advisory-data"]),
            [Evaluator] = new(
                VisibleAreas: ["evaluator-dashboard", "evaluation-queue"],
                AllowedActions: ["view-evaluation-content", "submit-evaluation"]),
            [Admin] = new(
                VisibleAreas: ["admin-dashboard", "role-management", "system-settings"],
                AllowedActions: ["assign-roles", "change-roles", "remove-roles"])
        };
}

public sealed record RoleCapabilitySet(
    IReadOnlyList<string> VisibleAreas,
    IReadOnlyList<string> AllowedActions);
