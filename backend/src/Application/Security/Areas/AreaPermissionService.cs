using CapabilityMap.Backend.Domain.Security;

namespace CapabilityMap.Backend.Application.Security.Areas;

public sealed class AreaPermissionService
{
    private readonly AreaManagementService _inner;

    public AreaPermissionService(AreaManagementService inner)
    {
        _inner = inner;
    }

    public AreaPermissionResponse Grant(Guid areaId, GrantPermissionRequest request, UserRoleContext actor)
    {
        return _inner.ManagePermission(
            areaId,
            new ManageAreaPermissionRequest(request.UserId, "grant", request.ChangeReason, request.ExpiresAt),
            actor);
    }

    public AreaPermissionResponse Revoke(Guid areaId, RevokePermissionRequest request, UserRoleContext actor)
    {
        return _inner.ManagePermission(
            areaId,
            new ManageAreaPermissionRequest(request.UserId, "revoke", request.ChangeReason),
            actor);
    }
}
