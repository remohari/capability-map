using CapabilityMap.Backend.Api.Controllers;
using CapabilityMap.Backend.Api.Middleware;
using CapabilityMap.Backend.Application.Security.RoleAssignments;
using CapabilityMap.Backend.Security;

namespace CapabilityMap.Backend.Api.Controllers;

public static class RoleAssignmentsController
{
    public const string Route = "/api/role-assignments";

    public static readonly string[] PlannedOperations =
    [
        "GET /api/role-assignments",
        "GET /api/role-assignments/{userId}",
        "PUT /api/role-assignments/{userId}",
        "DELETE /api/role-assignments/{userId}"
    ];

    public static IEndpointRouteBuilder MapRoleAssignmentEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(Route, (RoleAssignmentService service) =>
        {
            return Results.Ok(service.GetAll());
        });

        endpoints.MapGet($"{Route}/{{userId}}", (string userId, RoleAssignmentService service) =>
        {
            var result = service.Get(userId);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        endpoints.MapPut($"{Route}/{{userId}}", (
            string userId,
            RoleAssignmentRequest request,
            HttpRequest httpRequest,
            RoleAuthorizationService authorizationService,
            AuthorizationEventLogger auditLogger,
            RoleAssignmentService service) =>
        {
            var actor = RequestUserContextFactory.Create(httpRequest);
            var decision = authorizationService.EvaluateAccess(
                actor,
                target: "assign-roles",
                requiredRole: RoleDefinitions.Admin);

            if (decision.Result == Domain.Security.AuthorizationResult.Denied)
            {
                return AuthorizationHttpResponses.FromDecision(decision);
            }

            var result = service.Assign(userId, actor.DisplayReference, request);
            return Results.Ok(result);
        });

        endpoints.MapDelete($"{Route}/{{userId}}", (
            string userId,
            HttpRequest httpRequest,
            RoleAuthorizationService authorizationService,
            RoleAssignmentService service) =>
        {
            var actor = RequestUserContextFactory.Create(httpRequest);
            var decision = authorizationService.EvaluateAccess(
                actor,
                target: "remove-roles",
                requiredRole: RoleDefinitions.Admin);

            if (decision.Result == Domain.Security.AuthorizationResult.Denied)
            {
                return AuthorizationHttpResponses.FromDecision(decision);
            }

            return service.Remove(userId, actor.DisplayReference)
                ? Results.NoContent()
                : Results.NotFound();
        });

        endpoints.MapGet("/api/admin/audit-events", (RoleAssignmentService service) =>
        {
            return Results.Ok(service.GetAuditTrail());
        });

        return endpoints;
    }
}
