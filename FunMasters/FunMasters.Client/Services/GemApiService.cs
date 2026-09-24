using System.Net.Http.Json;
using FunMasters.Shared.DTOs;
using FunMasters.Shared.Services;

namespace FunMasters.Client.Services;

public class GemApiService(HttpClient http) : IGemApiService
{
    public async Task<ApiResult<int>> AwardGemAsync(AwardGemRequest request)
    {
        var response = await http.PostAsJsonAsync("/api/gems", request);
        return await response.Content.ReadFromJsonAsync<ApiResult<int>>()
            ?? ApiResult<int>.Fail("Failed to award gem");
    }
}
