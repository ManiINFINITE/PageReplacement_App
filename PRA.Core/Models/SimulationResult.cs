namespace PRA.Core.Models;

public class SimulationResult
{
    public required string AlgorithmName { get; init; }
    public List<SimulationStep> Steps { get; init; } = [];
    public int PageFaults { get; set; }
    public int PageHits { get; set; }

    public double HitRatio => Steps.Count == 0 ? 0 : (double)PageHits / Steps.Count;
}