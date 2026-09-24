using FunMasters.Data;
using FunMasters.Shared;
using FunMasters.Shared.DTOs;
using FunMasters.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace FunMasters.Services;

/// <remarks>
/// Takes <see cref="IDbContextFactory{TContext}"/> rather than the scoped context: the nav's
/// member roll renders on every page, so this runs concurrently with whatever query that page
/// is already making, and a shared DbContext cannot serve two operations at once.
/// </remarks>
public class MemberService(
    IDbContextFactory<ApplicationDbContext> dbFactory,
    AvatarStorage avatarStorage,
    BadgeStorage badgeStorage) : IMemberApiService
{
    /// <summary>
    /// The public roll of the Council.
    ///
    /// Four queries regardless of how many members there are: the criminal record is folded in
    /// memory rather than queried per member. Nothing here is denormalised — a stored offence
    /// count would need invalidating whenever a verdict is written or edited, a title concludes,
    /// a standing changes, an order changes, or a member registers.
    /// </summary>
    public async Task<List<MemberListItemDto>> GetMembersAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var users = await db.Users
            .Include(u => u.UserBadges)
            .ThenInclude(ub => ub.Badge)
            .ToListAsync();

        var finishedGames = await db.Suggestions
            .Where(s => s.Status == SuggestionStatus.Finished)
            .Select(s => new { s.Id, Cutoff = s.ActiveAtUtc ?? s.FinishedAtUtc!.Value })
            .ToListAsync();

        // Only the opening of each comment is needed to count three words, and Comment runs to
        // 50,000 characters. (A 512-character unbroken token would be misjudged; not plausible.)
        var ratings = await db.Ratings
            .Where(r => r.Suggestion!.Status == SuggestionStatus.Finished)
            .Select(r => new
            {
                r.RaterId,
                r.SuggestionId,
                Snippet = r.Comment == null ? null : r.Comment.Substring(0, 512)
            })
            .ToListAsync();

        var gemsByUser = await db.Gems
            .GroupBy(g => g.Rating!.RaterId)
            .Select(x => new { UserId = x.Key, Count = x.Count() })
            .ToDictionaryAsync(x => x.UserId, x => x.Count);

        var delivered = ratings.Select(r => (r.RaterId, r.SuggestionId)).ToHashSet();

        var weakByUser = ratings
            .Where(r => !OffenceRules.IsCommentSubstantial(r.Snippet))
            .GroupBy(r => r.RaterId)
            .ToDictionary(g => g.Key, g => g.Count());

        return users
            .Select(u => new MemberListItemDto
            {
                Id = u.Id,
                UserName = u.UserName ?? "Unknown",
                AvatarUrl = avatarStorage.GetPublicUrl(u.Id),
                CouncilStatus = u.CouncilStatus.ToString(),
                CycleOrder = u.CycleOrder,
                Badges = u.UserBadges.Select(ub => new UserBadgeDto
                {
                    BadgeId = ub.BadgeId,
                    Name = ub.Badge.Name,
                    Description = ub.Badge.Description,
                    ImageUrl = badgeStorage.GetPublicUrl(ub.BadgeId)
                }).ToList(),
                GemsCollected = gemsByUser.GetValueOrDefault(u.Id),
                MissedVerdicts = finishedGames.Count(g =>
                    OffenceRules.OwesVerdict(u, g.Cutoff) && !delivered.Contains((u.Id, g.Id))),
                WeakReviews = weakByUser.GetValueOrDefault(u.Id)
            })
            // Seated members in rotation order; everyone else alphabetically behind them.
            .OrderBy(m => m.CycleOrder > 0 ? 0 : 1)
            .ThenBy(m => m.CycleOrder)
            .ThenBy(m => m.UserName)
            .ToList();
    }
}
