using Microsoft.EntityFrameworkCore;

namespace FunMasters.Data;

[PrimaryKey(nameof(CycleNumber))]
public class Cycle
{
    public int CycleNumber { get; set; }
    public DateTime StartAtUtc { get; set; }
    public DateTime? EndAtUtc { get; set; }

    // Writer of the Cycle — the member whose verdicts drew the most gems across this rotation.
    // Settled once, a fortnight after the cycle closes, so late verdicts on the final title
    // still count. Snapshotted rather than derived: gems keep arriving, and a title already
    // awarded must not quietly change hands.
    public Guid? WriterOfTheCycleUserId { get; set; }
    public ApplicationUser? WriterOfTheCycle { get; set; }
    public int? WriterGemCount { get; set; }
    public DateTime? WriterSettledAtUtc { get; set; }

    public ICollection<Suggestion> Suggestions { get; set; } = [];
    public ICollection<CycleVote> Votes { get; set; } = [];
}