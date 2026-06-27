using System.Net;
using System.Net.Http.Json;
using CapabilityMap.Backend.Application.Security.Areas;

namespace CapabilityMap.Backend.Tests.Integration.Api;

public sealed class AreaEndpointsTests : IClassFixture<TestApplicationFactory>
{
    private readonly HttpClient _client;

    public AreaEndpointsTests(TestApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Admin_ShouldCreateArea()
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/areas")
        {
            Content = JsonContent.Create(new CreateAreaRequest("Contoso", "CONT-001", "Area-1", "desc")),
        };
        request.Headers.Add("X-User-Id", "admin-user");
        request.Headers.Add("X-Display-Reference", "admin@example.invalid");
        request.Headers.Add("X-Active-Role", "Admin");

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task NonAdmin_ShouldBeDeniedAreaCreation()
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/areas")
        {
            Content = JsonContent.Create(new CreateAreaRequest("Contoso", "CONT-002", "Area-2", null)),
        };
        request.Headers.Add("X-User-Id", "customer-user");
        request.Headers.Add("X-Display-Reference", "customer@example.invalid");
        request.Headers.Add("X-Active-Role", "Kund:in");

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task GrantedCustomer_ShouldSeeOnlyGrantedAreas()
    {
        using var createRequest = new HttpRequestMessage(HttpMethod.Post, "/api/areas")
        {
            Content = JsonContent.Create(new CreateAreaRequest("Contoso", "CONT-003", "Area-3", null)),
        };
        createRequest.Headers.Add("X-User-Id", "admin-user");
        createRequest.Headers.Add("X-Display-Reference", "admin@example.invalid");
        createRequest.Headers.Add("X-Active-Role", "Admin");

        var createResponse = await _client.SendAsync(createRequest);
        var area = await createResponse.Content.ReadFromJsonAsync<AreaResponse>();
        Assert.NotNull(area);

        using var grantRequest = new HttpRequestMessage(HttpMethod.Post, $"/api/areas/{area.AreaId}/permissions")
        {
            Content = JsonContent.Create(new ManageAreaPermissionRequest("customer-42", "grant", "integration grant")),
        };
        grantRequest.Headers.Add("X-User-Id", "admin-user");
        grantRequest.Headers.Add("X-Display-Reference", "admin@example.invalid");
        grantRequest.Headers.Add("X-Active-Role", "Admin");

        var grantResponse = await _client.SendAsync(grantRequest);
        Assert.Equal(HttpStatusCode.OK, grantResponse.StatusCode);

        using var listRequest = new HttpRequestMessage(HttpMethod.Get, "/api/areas");
        listRequest.Headers.Add("X-User-Id", "customer-42");
        listRequest.Headers.Add("X-Display-Reference", "customer42@example.invalid");
        listRequest.Headers.Add("X-Active-Role", "Kund:in");

        var listResponse = await _client.SendAsync(listRequest);
        Assert.Equal(HttpStatusCode.OK, listResponse.StatusCode);

        var areas = await listResponse.Content.ReadFromJsonAsync<List<AreaResponse>>();
        Assert.NotNull(areas);
        Assert.Single(areas);
        Assert.Equal(area.AreaId, areas[0].AreaId);
    }
}
