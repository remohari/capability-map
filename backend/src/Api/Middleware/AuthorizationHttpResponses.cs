using CapabilityMap.Backend.Domain.Security;

namespace CapabilityMap.Backend.Api.Middleware;

public static class AuthorizationHttpResponses
{
    public static IResult FromDecision(AuthorizationDecision decision)
    {
        if (decision.Result == AuthorizationResult.Allowed)
        {
            return Results.Ok(decision);
        }

        return Results.Json(
            new
            {
                error = "access_denied",
                target = decision.Target,
                reasonCode = decision.ReasonCode,
                role = decision.RoleName
            },
            statusCode: StatusCodes.Status403Forbidden);
    }
}
