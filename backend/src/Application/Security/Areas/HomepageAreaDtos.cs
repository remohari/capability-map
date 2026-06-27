namespace CapabilityMap.Backend.Application.Security.Areas;

public sealed record HomepageAreaTileResponse(
    Guid AreaId,
    Guid CustomerId,
    string AreaName,
    string AreaStatus,
    string NavigationTarget);

public sealed record HomepageAreasResponse(
    IReadOnlyCollection<HomepageAreaTileResponse> Items,
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages,
    bool IsEmpty);
