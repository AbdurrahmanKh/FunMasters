using FunMasters.Shared.DTOs;

namespace FunMasters.Shared.Services;

public interface IMemberApiService
{
    Task<List<MemberListItemDto>> GetMembersAsync();
}
