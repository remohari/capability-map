using System.Net;

namespace CapabilityMap.Backend.Tests.Integration.Security;

public sealed class CustomerAccessPolicyTests : IClassFixture<TestApplicationFactory>
{
    private readonly HttpClient _client;

    public CustomerAccessPolicyTests(TestApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CustomerScope_ShouldAllowMatchingCustomer()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/customer/content/customer-001");
        request.Headers.Add("X-User-Id", "customer-user");
        request.Headers.Add("X-Display-Reference", "customer@example.invalid");
        request.Headers.Add("X-Active-Role", "Kund:in");
        request.Headers.Add("X-Customer-Scope", "customer-001");

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CustomerScope_ShouldDenyDifferentCustomer()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/customer/content/customer-002");
        request.Headers.Add("X-User-Id", "customer-user");
        request.Headers.Add("X-Display-Reference", "customer@example.invalid");
        request.Headers.Add("X-Active-Role", "Kund:in");
        request.Headers.Add("X-Customer-Scope", "customer-001");

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}
