using CapabilityMap.Backend.Application.Security.Areas;
using CapabilityMap.Backend.Domain.Security;
using CapabilityMap.Backend.Infrastructure.Logging;
using CapabilityMap.Backend.Infrastructure.Persistence;
using CapabilityMap.Backend.Security;

namespace CapabilityMap.Backend.Tests.Unit.Security;

public sealed class AreaManagementServiceTests
{
    [Fact]
    public void CreateArea_ShouldPersistAreaWithCustomer()
    {
        var service = CreateService();
        var actor = new UserRoleContext("admin-1", "admin@example.invalid", RoleDefinitions.Admin, UserAccountStatus.Active, null);

        var result = service.CreateArea(
            new CreateAreaRequest("Acme GmbH", "ACME-001", "Vertrieb", "Sales area"),
            actor);

        Assert.Equal("Vertrieb", result.Name);
        Assert.Equal("Acme GmbH", result.CustomerName);
        Assert.NotEqual(Guid.Empty, result.AreaId);
    }

    [Fact]
    public void CreateArea_ShouldRejectDuplicateAreaWithinSameCustomer()
    {
        var service = CreateService();
        var actor = new UserRoleContext("admin-1", "admin@example.invalid", RoleDefinitions.Admin, UserAccountStatus.Active, null);

        service.CreateArea(
            new CreateAreaRequest("Acme GmbH", "ACME-001", "Vertrieb", null),
            actor);

        Assert.Throws<InvalidOperationException>(() =>
            service.CreateArea(new CreateAreaRequest("Acme GmbH", "ACME-001", "Vertrieb", null), actor));
    }

    [Fact]
    public void ListAreas_ShouldReturnOnlyGrantedAreasForNonAdmin()
    {
        var service = CreateService();
        var admin = new UserRoleContext("admin-1", "admin@example.invalid", RoleDefinitions.Admin, UserAccountStatus.Active, null);
        var customerUser = new UserRoleContext("user-1", "user@example.invalid", RoleDefinitions.Customer, UserAccountStatus.Active, null);

        var areaA = service.CreateArea(new CreateAreaRequest("Acme GmbH", "ACME-001", "Area A", null), admin);
        _ = service.CreateArea(new CreateAreaRequest("Acme GmbH", "ACME-001", "Area B", null), admin);

        service.ManagePermission(areaA.AreaId, new ManageAreaPermissionRequest("user-1", "grant", "test grant"), admin);

        var visibleAreas = service.GetAreasForUser(customerUser, null);

        Assert.Single(visibleAreas);
        Assert.Equal("Area A", visibleAreas[0].Name);
    }

    private static AreaManagementService CreateService()
    {
        return new AreaManagementService(new InMemoryAreaStore(), new AuthorizationEventLogger());
    }
}
