using System.Net;

namespace CapabilityMap.Backend.Tests.Integration.Api;

public sealed class CustomerProtectedEndpointsTests : IClassFixture<TestApplicationFactory>
{
    private readonly HttpClient _client;

    public CustomerProtectedEndpointsTests(TestApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CustomerEndpoint_ShouldRejectAdvisorRole()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/customer/content/customer-001");
        request.Headers.Add("X-User-Id", "advisor-user");
        request.Headers.Add("X-Display-Reference", "advisor@example.invalid");
        request.Headers.Add("X-Active-Role", "Berater:in");
        request.Headers.Add("X-Customer-Scope", "customer-001");

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}
