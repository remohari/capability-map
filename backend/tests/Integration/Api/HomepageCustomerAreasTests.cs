using System.Net;
using System.Net.Http.Json;
using CapabilityMap.Backend.Application.Security.Areas;

namespace CapabilityMap.Backend.Tests.Integration.Api;

public sealed class HomepageCustomerAreasTests : IClassFixture<TestApplicationFactory>
{
    private readonly HttpClient _client;

    public HomepageCustomerAreasTests(TestApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GrantedCustomer_ShouldReceiveOnlyAuthorizedHomepageAreas()
    {
        var area = await CreateAreaAsync("Contoso", "CONT-HOME-1", "Home-Area-1");
        await GrantAreaPermissionAsync(area.AreaId, "customer-home-user");

        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/home/customer-areas");
        request.Headers.Add("X-User-Id", "customer-home-user");
        request.Headers.Add("X-Display-Reference", "customer-home-user@example.invalid");
        request.Headers.Add("X-Active-Role", "Kund:in");

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var payload = await response.Content.ReadFromJsonAsync<HomepageAreasResponse>();
        Assert.NotNull(payload);
        Assert.False(payload.IsEmpty);
        Assert.Single(payload.Items);
        Assert.Equal(area.AreaId, payload.Items.Single().AreaId);
    }

    [Fact]
    public async Task HomepageAreas_ShouldSupportSearchAndPagination()
    {
        await CreateAreaAsync("Acme", "ACME-HOME-1", "Alpha-One");
        await CreateAreaAsync("Acme", "ACME-HOME-2", "Beta-Two");

        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/home/customer-areas?search=beta&page=1&pageSize=1");
        request.Headers.Add("X-User-Id", "admin-user");
        request.Headers.Add("X-Display-Reference", "admin@example.invalid");
        request.Headers.Add("X-Active-Role", "Admin");

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var payload = await response.Content.ReadFromJsonAsync<HomepageAreasResponse>();
        Assert.NotNull(payload);
        Assert.Equal(1, payload.Page);
        Assert.Equal(1, payload.PageSize);
        Assert.Equal(1, payload.TotalItems);
        Assert.Equal(1, payload.TotalPages);
        Assert.Single(payload.Items);
        Assert.Contains("Beta", payload.Items.Single().AreaName, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task HomepageAreas_ForUserWithoutPermissions_ShouldReturnEmptyEnvelope()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/home/customer-areas");
        request.Headers.Add("X-User-Id", "customer-without-access");
        request.Headers.Add("X-Display-Reference", "customer-without-access@example.invalid");
        request.Headers.Add("X-Active-Role", "Kund:in");

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var payload = await response.Content.ReadFromJsonAsync<HomepageAreasResponse>();
        Assert.NotNull(payload);
        Assert.True(payload.IsEmpty);
        Assert.Empty(payload.Items);
        Assert.Equal(0, payload.TotalItems);
    }

    private async Task<AreaResponse> CreateAreaAsync(string customerName, string customerIdentifier, string areaName)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/areas")
        {
            Content = JsonContent.Create(new CreateAreaRequest(customerName, customerIdentifier, areaName, null)),
        };
        request.Headers.Add("X-User-Id", "admin-user");
        request.Headers.Add("X-Display-Reference", "admin@example.invalid");
        request.Headers.Add("X-Active-Role", "Admin");

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var area = await response.Content.ReadFromJsonAsync<AreaResponse>();
        return area!;
    }

    private async Task GrantAreaPermissionAsync(Guid areaId, string userId)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, $"/api/areas/{areaId}/permissions")
        {
            Content = JsonContent.Create(new ManageAreaPermissionRequest(userId, "grant", "homepage access")),
        };
        request.Headers.Add("X-User-Id", "admin-user");
        request.Headers.Add("X-Display-Reference", "admin@example.invalid");
        request.Headers.Add("X-Active-Role", "Admin");

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}
