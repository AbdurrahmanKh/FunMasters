using FunMasters.Shared.DTOs;
using FunMasters.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace FunMasters.Endpoints;

public static class GemEndpoints
{
    public static RouteGroupBuilder MapGemEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/gems")
            .DisableAntiforgery();

        // POST /api/gems
        group.MapPost("/", async (
            [FromBody] AwardGemRequest request,
            IGemApiService service) =>
        {
            var result = await service.AwardGemAsync(request);
            return Results.Json(result);
        }).RequireAuthorization();

        return group;
    }
}
