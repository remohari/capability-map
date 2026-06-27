namespace CapabilityMap.Backend.Application.Security.Areas;

public sealed record CreateAreaRequest(
    string CustomerName,
    string CustomerIdentifier,
    string AreaName,
    string? AreaDescription);
