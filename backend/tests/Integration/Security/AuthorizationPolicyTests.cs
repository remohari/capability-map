using System.Net;
using System.Net.Http.Headers;

namespace CapabilityMap.Backend.Tests.Integration.Security;

public sealed class AuthorizationPolicyTests : IClassFixture<TestApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthorizationPolicyTests(TestApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ProtectedRoute_ShouldReturnDenied_WhenRoleIsMissing()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/customer/content/customer-001");
        request.Headers.Add("X-User-Id", "customer-user");
        request.Headers.Add("X-Display-Reference", "customer@example.invalid");
        request.Headers.Add("X-Customer-Scope", "customer-001");

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task ProtectedRoute_ShouldReturnDenied_WhenRoleIsInsufficient()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/advisor/cases");
        request.Headers.Add("X-User-Id", "customer-user");
        request.Headers.Add("X-Display-Reference", "customer@example.invalid");
        request.Headers.Add("X-Active-Role", "Kund:in");

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}
