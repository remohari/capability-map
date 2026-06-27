using CapabilityMap.Backend.Domain.Security;
using CapabilityMap.Backend.Infrastructure.Logging;
using CapabilityMap.Backend.Infrastructure.Persistence;
using CapabilityMap.Backend.Security;

namespace CapabilityMap.Backend.Application.Security.Areas;

public sealed class AreaManagementService
{
    private readonly IAreaStore _store;
    private readonly AuthorizationEventLogger _auditLogger;
    private readonly List<AuthorizationAuditEvent> _auditTrail = [];

    public AreaManagementService(IAreaStore store, AuthorizationEventLogger auditLogger)
    {
        _store = store;
        _auditLogger = auditLogger;
    }

    public AreaResponse CreateArea(CreateAreaRequest request, UserRoleContext actor)
    {
        if (string.IsNullOrWhiteSpace(request.CustomerName) ||
            string.IsNullOrWhiteSpace(request.CustomerIdentifier) ||
            string.IsNullOrWhiteSpace(request.AreaName))
        {
            throw new InvalidOperationException("Required fields are missing.");
        }

        var customer = _store.FindCustomerByIdentifier(request.CustomerIdentifier)
            ?? _store.UpsertCustomer(new Kunde(
                Guid.NewGuid(),
                request.CustomerName.Trim(),
                request.CustomerIdentifier.Trim(),
                "active",
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow));

        var duplicate = _store.GetAreas().Any(a =>
            a.CustomerId == customer.CustomerId &&
            string.Equals(a.Name, request.AreaName.Trim(), StringComparison.OrdinalIgnoreCase));

        if (duplicate)
        {
            throw new InvalidOperationException("An area with this name already exists for the specified customer.");
        }

        var area = _store.UpsertArea(new Bereich(
            Guid.NewGuid(),
            customer.CustomerId,
            request.AreaName.Trim(),
            request.AreaDescription?.Trim(),
            "active",
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow));

        _auditTrail.Add(_auditLogger.CreateRoleMutationEvent(
            "area_created",
            actor.DisplayReference,
            actor.UserId,
            area.Name));

        return new AreaResponse(area.AreaId, area.CustomerId, customer.Name, area.Name, area.Description, area.Status);
    }

    public IReadOnlyCollection<AreaResponse> GetAreasForUser(UserRoleContext user, string? requestedCustomerScope)
    {
        var customersById = _store.GetCustomers().ToDictionary(x => x.CustomerId);
        var areas = _store.GetAreas().AsEnumerable();

        if (!string.Equals(user.ActiveRole, RoleDefinitions.Admin, StringComparison.Ordinal))
        {
            areas = areas.Where(area => HasUserAccess(area.AreaId, user.UserId));
        }

        var scope = requestedCustomerScope ?? user.CustomerScope;
        if (!string.IsNullOrWhiteSpace(scope) && Guid.TryParse(scope, out var scopedCustomerId))
        {
            areas = areas.Where(a => a.CustomerId == scopedCustomerId);
        }

        return areas
            .Select(a => new AreaResponse(
                a.AreaId,
                a.CustomerId,
                customersById.TryGetValue(a.CustomerId, out var customer) ? customer.Name : "unknown",
                a.Name,
                a.Description,
                a.Status))
            .OrderBy(a => a.CustomerName, StringComparer.OrdinalIgnoreCase)
            .ThenBy(a => a.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    public HomepageAreasResponse GetHomepageAreas(
        UserRoleContext user,
        string? search,
        int page,
        int pageSize)
    {
        var normalizedSearch = search?.Trim();
        var scopedAreas = GetAreasForUser(user, requestedCustomerScope: null).AsEnumerable();

        if (!string.IsNullOrWhiteSpace(normalizedSearch))
        {
            scopedAreas = scopedAreas.Where(area =>
                area.Name.Contains(normalizedSearch, StringComparison.OrdinalIgnoreCase) ||
                area.CustomerName.Contains(normalizedSearch, StringComparison.OrdinalIgnoreCase));
        }

        var totalItems = scopedAreas.Count();
        var totalPages = totalItems == 0 ? 0 : (int)Math.Ceiling(totalItems / (double)pageSize);
        var items = scopedAreas
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(area => new HomepageAreaTileResponse(
                area.AreaId,
                area.CustomerId,
                area.Name,
                area.Status,
                $"/customer-areas/{area.AreaId}"))
            .ToArray();

        _auditTrail.Add(_auditLogger.CreateRoleMutationEvent(
            totalItems == 0 ? "areas_empty" : "areas_loaded",
            user.DisplayReference,
            user.UserId,
            "homepage-customer-areas"));

        return new HomepageAreasResponse(
            Items: items,
            Page: page,
            PageSize: pageSize,
            TotalItems: totalItems,
            TotalPages: totalPages,
            IsEmpty: totalItems == 0);
    }

    public AreaResponse? GetAreaById(Guid areaId, UserRoleContext user)
    {
        var area = _store.GetArea(areaId);
        if (area is null)
        {
            return null;
        }

        if (!string.Equals(user.ActiveRole, RoleDefinitions.Admin, StringComparison.Ordinal) &&
            !HasUserAccess(areaId, user.UserId))
        {
            return null;
        }

        if (!string.IsNullOrWhiteSpace(user.CustomerScope) &&
            Guid.TryParse(user.CustomerScope, out var scopedCustomerId) &&
            scopedCustomerId != area.CustomerId &&
            !string.Equals(user.ActiveRole, RoleDefinitions.Admin, StringComparison.Ordinal))
        {
            return null;
        }

        var customer = _store.GetCustomers().FirstOrDefault(c => c.CustomerId == area.CustomerId);
        return new AreaResponse(area.AreaId, area.CustomerId, customer?.Name ?? "unknown", area.Name, area.Description, area.Status);
    }

    public AreaPermissionResponse ManagePermission(Guid areaId, ManageAreaPermissionRequest request, UserRoleContext actor)
    {
        var area = _store.GetArea(areaId) ?? throw new InvalidOperationException("Area not found.");

        if (string.Equals(request.Action, "revoke", StringComparison.OrdinalIgnoreCase))
        {
            var existing = _store.FindActivePermission(areaId, request.UserId)
                ?? throw new InvalidOperationException("No active permission to revoke.");

            var revoked = existing with
            {
                Status = "revoked",
                ChangedAt = DateTimeOffset.UtcNow,
                ChangedBy = actor.DisplayReference,
                ChangeReason = request.ChangeReason
            };
            _store.UpsertPermission(revoked);

            _auditTrail.Add(_auditLogger.CreateRoleMutationEvent(
                "permission_revoked",
                actor.DisplayReference,
                request.UserId,
                area.Name));

            return ToPermissionResponse(revoked, "revoke");
        }

        var permission = _store.UpsertPermission(new Bereichsberechtigung(
            Guid.NewGuid(),
            areaId,
            request.UserId,
            "active",
            DateTimeOffset.UtcNow,
            actor.DisplayReference,
            request.ChangeReason,
            request.ExpiresAt));

        _auditTrail.Add(_auditLogger.CreateRoleMutationEvent(
            "permission_granted",
            actor.DisplayReference,
            request.UserId,
            area.Name));

        return ToPermissionResponse(permission, "grant");
    }

    public IReadOnlyCollection<AuthorizationAuditEvent> GetAuditTrail() => _auditTrail.AsReadOnly();

    private bool HasUserAccess(Guid areaId, string userId)
    {
        var permission = _store.FindActivePermission(areaId, userId);
        if (permission is null)
        {
            return false;
        }

        if (permission.ExpiresAt is { } expiresAt && expiresAt <= DateTimeOffset.UtcNow)
        {
            return false;
        }

        return true;
    }

    private static AreaPermissionResponse ToPermissionResponse(Bereichsberechtigung permission, string action)
    {
        return new AreaPermissionResponse(
            permission.PermissionId,
            permission.AreaId,
            permission.UserId,
            action,
            permission.GrantedAt,
            permission.ChangedBy,
            permission.ChangeReason,
            permission.ExpiresAt);
    }
}
