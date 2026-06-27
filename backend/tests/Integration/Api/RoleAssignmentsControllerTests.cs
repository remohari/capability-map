using System.Net;
using System.Net.Http.Json;
using CapabilityMap.Backend.Application.Security.RoleAssignments;
using CapabilityMap.Backend.Security;

namespace CapabilityMap.Backend.Tests.Integration.Api;

public sealed class RoleAssignmentsControllerTests : IClassFixture<TestApplicationFactory>
{
    private readonly HttpClient _client;

    public RoleAssignmentsControllerTests(TestApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Admin_ShouldAssignRole()
    {
        using var request = new HttpRequestMessage(HttpMethod.Put, "/api/role-assignments/customer-007")
        {
            Content = JsonContent.Create(new RoleAssignmentRequest(RoleDefinitions.Customer, "Integration test"))
        };
        request.Headers.Add("X-User-Id", "admin-user");
        request.Headers.Add("X-Display-Reference", "admin@example.invalid");
        request.Headers.Add("X-Active-Role", "Admin");

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var payload = await response.Content.ReadFromJsonAsync<RoleAssignmentResponse>();
        Assert.NotNull(payload);
        Assert.Equal(RoleDefinitions.Customer, payload.RoleName);
    }

    [Fact]
    public async Task NonAdmin_ShouldBeDeniedRoleAssignment()
    {
        using var request = new HttpRequestMessage(HttpMethod.Put, "/api/role-assignments/customer-008")
        {
            Content = JsonContent.Create(new RoleAssignmentRequest(RoleDefinitions.Customer, "Denied test"))
        };
        request.Headers.Add("X-User-Id", "advisor-user");
        request.Headers.Add("X-Display-Reference", "advisor@example.invalid");
        request.Headers.Add("X-Active-Role", "Berater:in");

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}
