namespace FunMasters.Data;

public static class CouncilStatusRoles
{
    public static readonly List<Shared.CouncilStatus> CanSuggest = [Shared.CouncilStatus.Active];
    public static readonly List<Shared.CouncilStatus> ExtraFloatingSuggestions = [Shared.CouncilStatus.Candidate, Shared.CouncilStatus.Excommunicated];
    public static readonly List<Shared.CouncilStatus> InQueue = [Shared.CouncilStatus.Active];
    public static readonly List<Shared.CouncilStatus> MustReview = [Shared.CouncilStatus.Active, Shared.CouncilStatus.Excommunicated];
    public static readonly List<Shared.CouncilStatus> ReceiveNotifications = [Shared.CouncilStatus.Active, Shared.CouncilStatus.Candidate, Shared.CouncilStatus.Excommunicated];
    public static readonly List<Shared.CouncilStatus> ShamingNotifications = [Shared.CouncilStatus.Active, Shared.CouncilStatus.Excommunicated];
}