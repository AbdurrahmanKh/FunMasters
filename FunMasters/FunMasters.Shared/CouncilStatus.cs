using System.Text.Json.Serialization;

namespace FunMasters.Shared;

[JsonConverter(typeof(JsonStringEnumConverter<CouncilStatus>))]
public enum CouncilStatus
{
    Active = 0,
    Candidate = 1,
    Excommunicated = 2,
    Executed = 3,
    Shadow = 4
}