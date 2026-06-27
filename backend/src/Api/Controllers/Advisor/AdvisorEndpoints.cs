using CapabilityMap.Backend.Api.Controllers;
using CapabilityMap.Backend.Api.Middleware;
using CapabilityMap.Backend.Security;

namespace CapabilityMap.Backend.Api.Controllers.Advisor;

public static class AdvisorEndpoints
{
    public static IEndpointRouteBuilder MapAdvisorEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/advisor/cases", (
            HttpRequest request,
            RoleAuthorizationService authorizationService) =>
        {
            var user = RequestUserContextFactory.Create(request);
            var decision = authorizationService.EvaluateAccess(
                user,
                target: "advisor-cases",
                requiredRole: RoleDefinitions.Advisor);

            if (decision.Result == Domain.Security.AuthorizationResult.Denied)
            {
                return AuthorizationHttpResponses.FromDecision(decision);
            }

            return Results.Ok(new
            {
                advisor = user.DisplayReference,
                cases =
                [
                    new { id = "case-001", title = "Foerderantrag pruefen", status = "in_beratung" },
                    new { id = "case-002", title = "Unterlagen nachreichen", status = "wartet_auf_kunde" }
                ]
            });
        });

        return endpoints;
    }
}
