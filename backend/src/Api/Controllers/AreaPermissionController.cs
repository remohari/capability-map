using CapabilityMap.Backend.Api.Middleware;
using CapabilityMap.Backend.Application.Security.Areas;
using CapabilityMap.Backend.Security;

namespace CapabilityMap.Backend.Api.Controllers;

public static class AreaPermissionController
{
    public static IEndpointRouteBuilder MapAreaPermissionEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/area-permissions/{areaId}/grant", (
            Guid areaId,
            GrantPermissionRequest request,
            HttpRequest httpRequest,
            RoleAuthorizationService authorizationService,
            AreaPermissionService service) =>
        {
            var actor = RequestUserContextFactory.Create(httpRequest);
            var decision = authorizationService.EvaluateAccess(actor, "manage-area-permissions", RoleDefinitions.Admin);
            if (decision.Result == Domain.Security.AuthorizationResult.Denied)
            {
                return AuthorizationHttpResponses.FromDecision(decision);
            }

            return Results.Ok(service.Grant(areaId, request, actor));
        });

        endpoints.MapPost("/api/area-permissions/{areaId}/revoke", (
            Guid areaId,
            RevokePermissionRequest request,
            HttpRequest httpRequest,
            RoleAuthorizationService authorizationService,
            AreaPermissionService service) =>
        {
            var actor = RequestUserContextFactory.Create(httpRequest);
            var decision = authorizationService.EvaluateAccess(actor, "manage-area-permissions", RoleDefinitions.Admin);
            if (decision.Result == Domain.Security.AuthorizationResult.Denied)
            {
                return AuthorizationHttpResponses.FromDecision(decision);
            }

            return Results.Ok(service.Revoke(areaId, request, actor));
        });

        return endpoints;
    }
}
