using System.Net;

namespace CapabilityMap.Backend.Tests.Integration.Security;

public sealed class AdvisorEvaluatorPolicyTests : IClassFixture<TestApplicationFactory>
{
    private readonly HttpClient _client;

    public AdvisorEvaluatorPolicyTests(TestApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Advisor_ShouldAccessAdvisorCases()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/advisor/cases");
        request.Headers.Add("X-User-Id", "advisor-user");
        request.Headers.Add("X-Display-Reference", "advisor@example.invalid");
        request.Headers.Add("X-Active-Role", "Berater:in");

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Evaluator_ShouldAccessEvaluationQueue()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/evaluator/queue");
        request.Headers.Add("X-User-Id", "evaluator-user");
        request.Headers.Add("X-Display-Reference", "evaluator@example.invalid");
        request.Headers.Add("X-Active-Role", "Bewerter:in");

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
