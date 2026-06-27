using CapabilityMap.Backend.Api.Controllers;
using CapabilityMap.Backend.Api.Middleware;
using CapabilityMap.Backend.Infrastructure.Logging;
using CapabilityMap.Backend.Security;

namespace CapabilityMap.Backend.Api.Controllers.Customer;

public static class CustomerContentEndpoints
{
    public static IEndpointRouteBuilder MapCustomerContentEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/customer/content/{customerId}", (
            string customerId,
            HttpRequest request,
            RoleAuthorizationService authorizationService,
            AuthorizationEventLogger auditLogger) =>
        {
            var user = RequestUserContextFactory.Create(request);
            var decision = authorizationService.EvaluateAccess(
                user,
                target: "customer-dashboard",
                requiredRole: RoleDefinitions.Customer,
                requiredCustomerScope: customerId);

            if (decision.Result == Domain.Security.AuthorizationResult.Denied)
            {
                return AuthorizationHttpResponses.FromDecision(decision);
            }

            return Results.Ok(new
            {
                customerId,
                owner = user.DisplayReference,
                items = new[]
                {
                    "Persoenliches Dashboard",
                    "Eigene Beratungshistorie",
                    "Freigegebene Dokumente"
                }
            });
        });

        return endpoints;
    }
}
