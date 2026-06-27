using System.Net;

namespace CapabilityMap.Backend.Tests.Integration.Api;

public sealed class HomepageCustomerAreasValidationTests : IClassFixture<TestApplicationFactory>
{
    private readonly HttpClient _client;

    public HomepageCustomerAreasValidationTests(TestApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task InvalidPageOrPageSize_ShouldReturnBadRequest()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/home/customer-areas?page=0&pageSize=999");
        request.Headers.Add("X-User-Id", "admin-user");
        request.Headers.Add("X-Display-Reference", "admin@example.invalid");
        request.Headers.Add("X-Active-Role", "Admin");

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
