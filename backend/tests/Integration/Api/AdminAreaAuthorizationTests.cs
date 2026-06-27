using System.Net;

namespace CapabilityMap.Backend.Tests.Integration.Api;

public sealed class AdminAreaAuthorizationTests : IClassFixture<TestApplicationFactory>
{
    private readonly HttpClient _client;

    public AdminAreaAuthorizationTests(TestApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Advisor_ShouldBeDeniedAdminRoleManagement()
    {
        using var request = new HttpRequestMessage(HttpMethod.Put, "/api/role-assignments/user-x")
        {
            Content = JsonContent.Create(new { roleName = "Kund:in", changeReason = "Denied" })
        };
        request.Headers.Add("X-User-Id", "advisor-user");
        request.Headers.Add("X-Display-Reference", "advisor@example.invalid");
        request.Headers.Add("X-Active-Role", "Berater:in");

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Evaluator_ShouldBeDeniedAdminRoleManagement()
    {
        using var request = new HttpRequestMessage(HttpMethod.Put, "/api/role-assignments/user-y")
        {
            Content = JsonContent.Create(new { roleName = "Kund:in", changeReason = "Denied" })
        };
        request.Headers.Add("X-User-Id", "evaluator-user");
        request.Headers.Add("X-Display-Reference", "evaluator@example.invalid");
        request.Headers.Add("X-Active-Role", "Bewerter:in");

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}
