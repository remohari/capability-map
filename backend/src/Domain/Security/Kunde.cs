namespace CapabilityMap.Backend.Domain.Security;

public sealed record Kunde(
    Guid CustomerId,
    string Name,
    string Identifier,
    string Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
