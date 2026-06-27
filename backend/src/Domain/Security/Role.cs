namespace CapabilityMap.Backend.Domain.Security;

public sealed record Role(
    string Name,
    string Description,
    IReadOnlyList<string> AllowedAreas,
    IReadOnlyList<string> AllowedActions,
    bool IsAssignable = true);
