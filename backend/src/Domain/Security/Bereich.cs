namespace CapabilityMap.Backend.Domain.Security;

public sealed record Bereich(
    Guid AreaId,
    Guid CustomerId,
    string Name,
    string? Description,
    string Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
