namespace CapabilityMap.Backend.Application.Security.Areas;

public sealed record CreateAreaResponse(
    Guid AreaId,
    Guid CustomerId,
    string CustomerName,
    string Name,
    string? Description,
    string Status);
