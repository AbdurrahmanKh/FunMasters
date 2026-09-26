using FunMasters.Shared.DTOs;

namespace FunMasters.Shared.Services;

public interface IGemApiService
{
    /// <summary>
    /// Bestows the caller's gem for this title upon a review. Returns the review's new gem count.
    /// There is deliberately no counterpart to withdraw it.
    /// </summary>
    Task<ApiResult<int>> AwardGemAsync(AwardGemRequest request);
}
