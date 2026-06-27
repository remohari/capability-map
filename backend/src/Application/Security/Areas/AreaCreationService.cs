using CapabilityMap.Backend.Domain.Security;

namespace CapabilityMap.Backend.Application.Security.Areas;

public sealed class AreaCreationService
{
    private readonly AreaManagementService _inner;

    public AreaCreationService(AreaManagementService inner)
    {
        _inner = inner;
    }

    public CreateAreaResponse CreateArea(CreateAreaRequest request, UserRoleContext actor)
    {
        var area = _inner.CreateArea(request, actor);
        return new CreateAreaResponse(
            area.AreaId,
            area.CustomerId,
            area.CustomerName,
            area.Name,
            area.Description,
            area.Status);
    }
}
