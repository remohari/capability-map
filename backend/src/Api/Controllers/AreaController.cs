using CapabilityMap.Backend.Api.Middleware;
using CapabilityMap.Backend.Application.Security.Areas;
using CapabilityMap.Backend.Security;

namespace CapabilityMap.Backend.Api.Controllers;

public static class AreaController
{
    public const string Route = "/api/areas";

    public static IEndpointRouteBuilder MapAreaEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(Route, (
            CreateAreaRequest request,
            HttpRequest httpRequest,
            RoleAuthorizationService authorizationService,
            AreaManagementService service) =>
        {
            var actor = RequestUserContextFactory.Create(httpRequest);
            var decision = authorizationService.EvaluateAccess(
                actor,
                target: "create-area",
                requiredRole: RoleDefinitions.Admin);

            if (decision.Result == Domain.Security.AuthorizationResult.Denied)
            {
                return AuthorizationHttpResponses.FromDecision(decision);
            }

            var result = service.CreateArea(request, actor);
            return Results.Created($"{Route}/{result.AreaId}", result);
        });

        endpoints.MapGet(Route, (
            string? customerId,
            HttpRequest httpRequest,
            RoleAuthorizationService authorizationService,
            AreaManagementService service) =>
        {
            var actor = RequestUserContextFactory.Create(httpRequest);
            var decision = authorizationService.EvaluateAccess(actor, target: "list-areas");

            if (decision.Result == Domain.Security.AuthorizationResult.Denied)
            {
                return AuthorizationHttpResponses.FromDecision(decision);
            }

            var results = service.GetAreasForUser(actor, customerId);
            return Results.Ok(results);
        });

        endpoints.MapGet("/api/home/customer-areas", (
            string? search,
            int? page,
            int? pageSize,
            HttpRequest httpRequest,
            RoleAuthorizationService authorizationService,
            AreaManagementService service) =>
        {
            var actor = RequestUserContextFactory.Create(httpRequest);
            var decision = authorizationService.EvaluateAccess(actor, target: "list-areas");

            if (decision.Result == Domain.Security.AuthorizationResult.Denied)
            {
                return AuthorizationHttpResponses.FromDecision(decision);
            }

            var normalizedPage = page ?? 1;
            var normalizedPageSize = pageSize ?? 20;
            if (normalizedPage < 1 || normalizedPageSize < 1 || normalizedPageSize > 100)
            {
                return Results.BadRequest(new
                {
                    type = "validation_error",
                    title = "Ungueltige Such- oder Paginierungsparameter",
                    detail = "Provided query parameters are invalid."
                });
            }

            var result = service.GetHomepageAreas(actor, search, normalizedPage, normalizedPageSize);
            return Results.Ok(result);
        });

        endpoints.MapGet($"{Route}/{{areaId}}", (
            Guid areaId,
            HttpRequest httpRequest,
            RoleAuthorizationService authorizationService,
            AreaManagementService service) =>
        {
            var actor = RequestUserContextFactory.Create(httpRequest);
            var decision = authorizationService.EvaluateAccess(actor, target: "view-area");

            if (decision.Result == Domain.Security.AuthorizationResult.Denied)
            {
                return AuthorizationHttpResponses.FromDecision(decision);
            }

            var result = service.GetAreaById(areaId, actor);
            return result is null ? Results.Forbid() : Results.Ok(result);
        });

        endpoints.MapPost($"{Route}/{{areaId}}/permissions", (
            Guid areaId,
            ManageAreaPermissionRequest request,
            HttpRequest httpRequest,
            RoleAuthorizationService authorizationService,
            AreaManagementService service) =>
        {
            var actor = RequestUserContextFactory.Create(httpRequest);
            var decision = authorizationService.EvaluateAccess(
                actor,
                target: "manage-area-permissions",
                requiredRole: RoleDefinitions.Admin);

            if (decision.Result == Domain.Security.AuthorizationResult.Denied)
            {
                return AuthorizationHttpResponses.FromDecision(decision);
            }

            var result = service.ManagePermission(areaId, request, actor);
            return Results.Ok(result);
        });

        return endpoints;
    }
}
