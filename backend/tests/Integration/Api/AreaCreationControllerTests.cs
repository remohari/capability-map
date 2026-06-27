using System.Net;
using System.Net.Http.Json;
using CapabilityMap.Backend.Application.Security.Areas;

namespace CapabilityMap.Backend.Tests.Integration.Api;

public sealed class AreaCreationControllerTests : IClassFixture<TestApplicationFactory>
{
    private readonly HttpClient _client;

    public AreaCreationControllerTests(TestApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostArea_ShouldCreateWhenAdmin()
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/areas")
        {
            Content = JsonContent.Create(new CreateAreaRequest("Acme", "AC-1", "Ops", null)),
        };
        request.Headers.Add("X-User-Id", "admin-user");
        request.Headers.Add("X-Display-Reference", "admin@example.invalid");
        request.Headers.Add("X-Active-Role", "Admin");

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}
