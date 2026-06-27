using CapabilityMap.Backend.Application.Security.RoleAssignments;
using CapabilityMap.Backend.Infrastructure.Logging;
using CapabilityMap.Backend.Infrastructure.Persistence;
using CapabilityMap.Backend.Security;

namespace CapabilityMap.Backend.Tests.Unit.Security;

public sealed class RoleAssignmentServiceTests
{
    [Fact]
    public void Assign_ShouldCreateSingleActiveRole()
    {
        var service = CreateService();

        var result = service.Assign("user-1", "admin@example.invalid", new RoleAssignmentRequest(RoleDefinitions.Customer, "Initial"));

        Assert.Equal("user-1", result.UserId);
        Assert.Equal(RoleDefinitions.Customer, result.RoleName);
    }

    [Fact]
    public void Assign_ShouldReplaceExistingRoleForSameUser()
    {
        var service = CreateService();
        service.Assign("user-1", "admin@example.invalid", new RoleAssignmentRequest(RoleDefinitions.Customer));

        var result = service.Assign("user-1", "admin@example.invalid", new RoleAssignmentRequest(RoleDefinitions.Evaluator));

        Assert.Equal(RoleDefinitions.Evaluator, result.RoleName);
        Assert.Single(service.GetAll());
    }

    [Fact]
    public void Remove_ShouldDeleteExistingAssignment()
    {
        var service = CreateService();
        service.Assign("user-1", "admin@example.invalid", new RoleAssignmentRequest(RoleDefinitions.Customer));

        var removed = service.Remove("user-1", "admin@example.invalid");

        Assert.True(removed);
        Assert.Empty(service.GetAll());
    }

    private static RoleAssignmentService CreateService()
    {
        return new RoleAssignmentService(new InMemoryRoleAssignmentStore(), new AuthorizationEventLogger());
    }
}
