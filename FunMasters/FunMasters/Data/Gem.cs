using Microsoft.EntityFrameworkCore;

namespace FunMasters.Data;

/// <summary>
/// A gem one member awards to another's verdict. Each member holds exactly one gem per title
/// and may never bestow it upon their own review; once given it cannot be withdrawn.
/// </summary>
[Index(nameof(SuggestionId), nameof(AwardedById), IsUnique = true)]
public class Gem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid RatingId { get; set; }
    public Rating? Rating { get; set; }

    /// <summary>
    /// Denormalised from <see cref="Rating"/> on purpose: it is what lets "one gem per member
    /// per game" be a unique index in the database rather than a rule the service must remember.
    /// Only ever set from the loaded Rating, never from a request.
    /// </summary>
    public Guid SuggestionId { get; set; }
    public Suggestion? Suggestion { get; set; }

    public Guid AwardedById { get; set; }
    public ApplicationUser? AwardedBy { get; set; }

    public DateTime AwardedAtUtc { get; set; } = DateTime.UtcNow;
}
