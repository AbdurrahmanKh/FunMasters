namespace FunMasters.Shared.DTOs;

public class CycleAdminDto
{
    public int CycleNumber { get; set; }
    public DateTime StartAtUtc { get; set; }
    public DateTime? EndAtUtc { get; set; }
    public int GameCount { get; set; }

    /// <summary>Gems awarded on reviews of this cycle's titles, as things stand right now.</summary>
    public int CurrentGemCount { get; set; }

    /// <summary>
    /// Gems that arrived on this cycle's reviews after its Writer was named. Any at all means the
    /// settled result may no longer reflect the tally, and the cycle is worth recomputing.
    /// </summary>
    public int GemsSinceSettled { get; set; }

    public string? WriterUserName { get; set; }
    public int? WriterGemCount { get; set; }
    public DateTime? WriterSettledAtUtc { get; set; }

    public bool IsOpen => EndAtUtc == null;

    public bool IsStale => GemsSinceSettled > 0;
}
