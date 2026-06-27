using System.Collections.Concurrent;
using CapabilityMap.Backend.Domain.Security;

namespace CapabilityMap.Backend.Infrastructure.Persistence;

public interface IAreaStore
{
    IReadOnlyCollection<Kunde> GetCustomers();
    Kunde? FindCustomerByIdentifier(string identifier);
    Kunde UpsertCustomer(Kunde customer);

    IReadOnlyCollection<Bereich> GetAreas();
    Bereich? GetArea(Guid areaId);
    Bereich UpsertArea(Bereich area);

    IReadOnlyCollection<Bereichsberechtigung> GetPermissions(Guid areaId);
    Bereichsberechtigung UpsertPermission(Bereichsberechtigung permission);
    Bereichsberechtigung? FindActivePermission(Guid areaId, string userId);
}

public sealed class InMemoryAreaStore : IAreaStore
{
    private readonly ConcurrentDictionary<Guid, Kunde> _customers = new();
    private readonly ConcurrentDictionary<Guid, Bereich> _areas = new();
    private readonly ConcurrentDictionary<Guid, Bereichsberechtigung> _permissions = new();

    public IReadOnlyCollection<Kunde> GetCustomers() => _customers.Values.ToArray();

    public Kunde? FindCustomerByIdentifier(string identifier)
    {
        return _customers.Values.FirstOrDefault(c =>
            string.Equals(c.Identifier, identifier, StringComparison.OrdinalIgnoreCase));
    }

    public Kunde UpsertCustomer(Kunde customer)
    {
        _customers[customer.CustomerId] = customer;
        return customer;
    }

    public IReadOnlyCollection<Bereich> GetAreas() => _areas.Values.ToArray();

    public Bereich? GetArea(Guid areaId)
    {
        _areas.TryGetValue(areaId, out var area);
        return area;
    }

    public Bereich UpsertArea(Bereich area)
    {
        _areas[area.AreaId] = area;
        return area;
    }

    public IReadOnlyCollection<Bereichsberechtigung> GetPermissions(Guid areaId)
    {
        return _permissions.Values.Where(p => p.AreaId == areaId).ToArray();
    }

    public Bereichsberechtigung UpsertPermission(Bereichsberechtigung permission)
    {
        _permissions[permission.PermissionId] = permission;
        return permission;
    }

    public Bereichsberechtigung? FindActivePermission(Guid areaId, string userId)
    {
        return _permissions.Values.FirstOrDefault(p =>
            p.AreaId == areaId &&
            string.Equals(p.UserId, userId, StringComparison.Ordinal) &&
            string.Equals(p.Status, "active", StringComparison.Ordinal));
    }
}
