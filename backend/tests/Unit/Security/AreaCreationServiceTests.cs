using CapabilityMap.Backend.Application.Security.Areas;
using CapabilityMap.Backend.Domain.Security;
using CapabilityMap.Backend.Infrastructure.Logging;
using CapabilityMap.Backend.Infrastructure.Persistence;
using CapabilityMap.Backend.Security;

namespace CapabilityMap.Backend.Tests.Unit.Security;

public sealed class AreaCreationServiceTests
{
    [Fact]
    public void Create_ShouldCreateAreaForCustomerContext()
    {
        var management = new AreaManagementService(new InMemoryAreaStore(), new AuthorizationEventLogger());
        var service = new AreaCreationService(management);
        var actor = new UserRoleContext("admin-1", "admin@example.invalid", RoleDefinitions.Admin, UserAccountStatus.Active, null);

        var result = service.CreateArea(new CreateAreaRequest("Demo", "DEMO-1", "Alpha", null), actor);

        Assert.Equal("Alpha", result.Name);
        Assert.Equal("Demo", result.CustomerName);
    }
}
