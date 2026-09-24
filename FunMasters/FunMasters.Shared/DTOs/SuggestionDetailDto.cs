namespace FunMasters.Shared.DTOs;

public class SuggestionDetailDto
{
    public SuggestionDto Suggestion { get; set; } = null!;
    public List<RatingDto> Ratings { get; set; } = [];
    public List<SteamPlaytimeDto> NonReviewerPlaytimes { get; set; } = [];

    /// <summary>The review the viewer already gemmed for this title, if they have spent it.</summary>
    public Guid? MyGemRatingId { get; set; }

    /// <summary>True when the viewer still holds an unspent gem they are entitled to award here.</summary>
    public bool CanAwardGem { get; set; }
}
