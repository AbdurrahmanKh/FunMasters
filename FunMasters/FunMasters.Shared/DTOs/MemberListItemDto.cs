namespace FunMasters.Shared.DTOs;

/// <summary>
/// A member as the public roll shows them. Deliberately not <see cref="UserDto"/>, which carries
/// the email address and is only ever served behind the admin policy.
/// </summary>
public class MemberListItemDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = null!;
    public string? AvatarUrl { get; set; }
    public string CouncilStatus { get; set; } = "Active";
    public int CycleOrder { get; set; }
    public List<UserBadgeDto> Badges { get; set; } = [];

    /// <summary>Gems this member's reviews have drawn, all time.</summary>
    public int GemsCollected { get; set; }

    /// <summary>Concluded titles they never delivered a verdict on.</summary>
    public int MissedVerdicts { get; set; }

    /// <summary>Verdicts delivered without the substance befitting a Fun Master's review.</summary>
    public int WeakReviews { get; set; }

    public int CriminalRecordNumber => MissedVerdicts + WeakReviews;
}
