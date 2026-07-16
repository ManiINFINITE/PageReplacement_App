namespace PRA.Core.Models;

public class SimulationStep
{
    public required int CurrentPage { get; init; }
    public List<int?> Frames { get; init; } = [];
    public bool IsPageFault { get; init; }
    public int? ReplacedPage { get; init; }

    // Clock
    public List<bool>? ReferenceBits { get; init; }
    public int? ClockPointer { get; init; }

    // LFU
    public List<int>? Frequencies { get; init; }
}