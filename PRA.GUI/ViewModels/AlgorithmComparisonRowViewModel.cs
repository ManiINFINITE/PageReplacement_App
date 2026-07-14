using Avalonia.Media;
using PRA.Core.Models;

namespace PRA.GUI.ViewModels;

public class AlgorithmComparisonRowViewModel(SimulationResult result, bool isBest, IBrush accentBrush) {

    public string AlgorithmName => result.AlgorithmName;
    public int PageHits => result.PageHits;
    public int PageFaults => result.PageFaults;
    public string HitRatioLabel => $"{result.HitRatio:P0}";
    public double HitRatioPercent => result.HitRatio * 100;
    public bool IsBest => isBest;
    public IBrush AccentBrush => accentBrush;

}
