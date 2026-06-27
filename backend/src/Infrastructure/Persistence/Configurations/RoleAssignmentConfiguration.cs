using CapabilityMap.Backend.Domain.Security;

namespace CapabilityMap.Backend.Infrastructure.Persistence.Configurations;

// Placeholder persistence mapping for the in-memory implementation.
// This keeps the file path aligned with the feature plan until a real EF model is added.
public sealed class RoleAssignmentConfiguration
{
    public string EntityName => nameof(RoleAssignment);
    public string PrimaryKey => nameof(RoleAssignment.AssignmentId);
    public string CurrentRoleField => nameof(RoleAssignment.RoleName);
}
