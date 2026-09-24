using System.Net.Http.Json;
using FunMasters.Shared.DTOs;
using FunMasters.Shared.Services;

namespace FunMasters.Client.Services;

public class MemberApiService(HttpClient http) : IMemberApiService
{
    public async Task<List<MemberListItemDto>> GetMembersAsync()
    {
        return await http.GetFromJsonAsync<List<MemberListItemDto>>("/api/members")
            ?? [];
    }
}
