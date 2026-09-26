using FunMasters.Shared.Services;

namespace FunMasters.Endpoints;

public static class MemberEndpoints
{
    public static RouteGroupBuilder MapMemberEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/members")
            .DisableAntiforgery();

        // GET /api/members — public, like the profiles it links to. Carries no email addresses.
        group.MapGet("/", async (IMemberApiService service) =>
        {
            var result = await service.GetMembersAsync();
            return Results.Ok(result);
        });

        return group;
    }
}
