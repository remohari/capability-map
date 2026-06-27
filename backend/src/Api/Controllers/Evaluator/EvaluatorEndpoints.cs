using CapabilityMap.Backend.Api.Controllers;
using CapabilityMap.Backend.Api.Middleware;
using CapabilityMap.Backend.Security;

namespace CapabilityMap.Backend.Api.Controllers.Evaluator;

public static class EvaluatorEndpoints
{
    public static IEndpointRouteBuilder MapEvaluatorEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/evaluator/queue", (
            HttpRequest request,
            RoleAuthorizationService authorizationService) =>
        {
            var user = RequestUserContextFactory.Create(request);
            var decision = authorizationService.EvaluateAccess(
                user,
                target: "evaluation-queue",
                requiredRole: RoleDefinitions.Evaluator);

            if (decision.Result == Domain.Security.AuthorizationResult.Denied)
            {
                return AuthorizationHttpResponses.FromDecision(decision);
            }

            return Results.Ok(new
            {
                evaluator = user.DisplayReference,
                queue =
                [
                    new { id = "eval-001", title = "Antrag A", priority = "hoch" },
                    new { id = "eval-002", title = "Antrag B", priority = "mittel" }
                ]
            });
        });

        return endpoints;
    }
}
