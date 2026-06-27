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
                VisibleAreas: ["customer-dashboard", "customer-profile", "list-areas", "view-area"],
                AllowedActions: ["view-own-content", "use-own-functions", "list-areas", "view-area"]),
            [Advisor] = new(
                VisibleAreas: ["advisor-dashboard", "advisor-cases", "list-areas", "view-area"],
                AllowedActions: ["view-assigned-cases", "edit-advisory-data", "list-areas", "view-area"]),
            [Evaluator] = new(
                VisibleAreas: ["evaluator-dashboard", "evaluation-queue", "list-areas", "view-area"],
                AllowedActions: ["view-evaluation-content", "submit-evaluation", "list-areas", "view-area"]),
            [Admin] = new(
                VisibleAreas: ["admin-dashboard", "role-management", "system-settings", "list-areas", "view-area"],
                AllowedActions: ["assign-roles", "change-roles", "remove-roles", "create-area", "manage-area-permissions", "list-areas", "view-area"])
        };
}

public sealed record RoleCapabilitySet(
    IReadOnlyList<string> VisibleAreas,
    IReadOnlyList<string> AllowedActions);
