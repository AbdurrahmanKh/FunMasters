using System.Security.Claims;
using FunMasters.Data;
using FunMasters.Shared;
using FunMasters.Shared.DTOs;
using FunMasters.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace FunMasters.Services;

public class GemService(
    ApplicationDbContext db,
    IHttpContextAccessor httpContextAccessor) : IGemApiService
{
    public async Task<ApiResult<int>> AwardGemAsync(AwardGemRequest request)
    {
        var userId = GetCurrentUserId();

        var rating = await db.Ratings
            .Include(r => r.Suggestion)
            .FirstOrDefaultAsync(r => r.Id == request.RatingId);

        if (rating == null)
            return ApiResult<int>.Fail("Review not found");

        if (rating.Suggestion?.Status != SuggestionStatus.Finished)
            return ApiResult<int>.Fail("Gems may only be awarded once deliberation has concluded");

        if (rating.RaterId == userId)
            return ApiResult<int>.Fail("You cannot award your gem to your own review");

        var iDeliveredAVerdict = await db.Ratings
            .AnyAsync(r => r.SuggestionId == rating.SuggestionId && r.RaterId == userId);

        if (!iDeliveredAVerdict)
            return ApiResult<int>.Fail("Only members who delivered a verdict on this game may award a gem");

        var alreadySpent = await db.Gems
            .AnyAsync(g => g.SuggestionId == rating.SuggestionId && g.AwardedById == userId);

        if (alreadySpent)
            return ApiResult<int>.Fail("You have already awarded your gem for this game");

        db.Gems.Add(new Gem
        {
            RatingId = rating.Id,
            // Taken from the loaded rating, never from the request, so it cannot drift.
            SuggestionId = rating.SuggestionId,
            AwardedById = userId
        });

        await db.SaveChangesAsync();

        return ApiResult<int>.Ok(await db.Gems.CountAsync(g => g.RatingId == rating.Id));
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            throw new UnauthorizedAccessException("User is not authenticated");
        return userId;
    }
}
