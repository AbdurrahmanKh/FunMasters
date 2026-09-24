using FunMasters.Data;

namespace FunMasters.Services;

/// <summary>
/// The Council's definitions of a delinquent verdict, in one place.
/// Both the reminder job and the criminal record on a member's profile read from here,
/// so what Lucian shames a member for and what their record shows can never drift apart.
/// </summary>
public static class OffenceRules
{
    public const int MinReviewWords = 3;

    /// <summary>
    /// A verdict carries substance when it has at least <see cref="MinReviewWords"/> words.
    /// </summary>
    public static bool IsCommentSubstantial(string? comment)
    {
        if (string.IsNullOrWhiteSpace(comment)) return false;
        return comment.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length >= MinReviewWords;
    }

    /// <summary>
    /// The moment a member had to have joined by to owe a verdict on this game —
    /// when it went before the Council, or failing that when it concluded.
    /// </summary>
    public static DateTime Cutoff(Suggestion suggestion) =>
        suggestion.ActiveAtUtc ?? suggestion.FinishedAtUtc!.Value;

    /// <summary>
    /// A member owes a verdict on a concluded game when they hold a seat in the rotation,
    /// their standing obliges them to review, and they had joined before the game went active.
    /// </summary>
    public static bool OwesVerdict(ApplicationUser user, DateTime gameCutoff) =>
        user.CycleOrder > 0
        && CouncilStatusRoles.MustReview.Contains(user.CouncilStatus)
        && user.RegistrationDateUtc <= gameCutoff;
}
