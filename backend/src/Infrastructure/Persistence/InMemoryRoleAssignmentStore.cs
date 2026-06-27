using System.Collections.Concurrent;
using CapabilityMap.Backend.Domain.Security;

namespace CapabilityMap.Backend.Infrastructure.Persistence;

public interface IRoleAssignmentStore
{
    RoleAssignment? Get(string userId);
    IReadOnlyCollection<RoleAssignment> GetAll();
    RoleAssignment Upsert(RoleAssignment assignment);
    RoleAssignment? Remove(string userId);
}

public sealed class InMemoryRoleAssignmentStore : IRoleAssignmentStore
{
    private readonly ConcurrentDictionary<string, RoleAssignment> _assignments = new(StringComparer.Ordinal);

    public RoleAssignment? Get(string userId)
    {
        _assignments.TryGetValue(userId, out var assignment);
        return assignment;
    }

    public IReadOnlyCollection<RoleAssignment> GetAll()
    {
        return _assignments.Values.OrderBy(x => x.UserId, StringComparer.Ordinal).ToArray();
    }

    public RoleAssignment Upsert(RoleAssignment assignment)
    {
        _assignments[assignment.UserId] = assignment;
        return assignment;
    }

    public RoleAssignment? Remove(string userId)
    {
        _assignments.TryRemove(userId, out var removed);
        return removed;
    }
}
