using System.Net.Http.Json;
using FunMasters.Shared.DTOs;
using FunMasters.Shared.Services;

namespace FunMasters.Client.Services;

public class GemApiService(HttpClient http) : IGemApiService
{
    public async Task<ApiResult<int>> AwardGemAsync(AwardGemRequest request)
    {
        var response = await http.PostAsJsonAsync("/api/gems", request);

        // An expired session returns 401 and a non-JSON body; deserialising that throws, and the
        // gem button is irreversible enough that it must never take the page down.
        if (!response.IsSuccessStatusCode)
        {
            return ApiResult<int>.Fail(response.StatusCode == System.Net.HttpStatusCode.Unauthorized
                ? "Your session has expired — sign in again to award your gem"
                : "The Council could not record your gem. Please try again.");
        }

        try
        {
            return await response.Content.ReadFromJsonAsync<ApiResult<int>>()
                   ?? ApiResult<int>.Fail("Failed to award gem");
        }
        catch (Exception)
        {
            return ApiResult<int>.Fail("Failed to award gem");
        }
    }
}
