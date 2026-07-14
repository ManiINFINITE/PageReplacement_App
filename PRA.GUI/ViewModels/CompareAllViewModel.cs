using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PRA.GUI.Models;
using PRA.GUI.Services;

namespace PRA.GUI.ViewModels;

/// <summary>
/// Runs every algorithm in the catalog against the same reference string and
/// frame count, then shows them ranked side by side. Input is collected via
/// an overlay (see CompareAllInputViewModel) rather than an inline form.
/// </summary>
public partial class CompareAllViewModel : ViewModelBase {

    // One accent color per algorithm, assigned by catalog position so a given
    // algorithm keeps the same color even as rows get re-sorted by faults.
    private static readonly IBrush[] Palette = [
        new SolidColorBrush(Color.Parse("#EF6448")), // Fire Opal
        new SolidColorBrush(Color.Parse("#5FD98A")), // Mint Green
        new SolidColorBrush(Color.Parse("#F2C14E")), // Amber
        new SolidColorBrush(Color.Parse("#5AC8FA")), // Sky
        new SolidColorBrush(Color.Parse("#F0455A")), // Crimson
    ];

    private readonly IOverlayService _overlay;

    [ObservableProperty] private bool hasResults;
    [ObservableProperty] private string resultsSummaryLabel = "";

    public ObservableCollection<AlgorithmComparisonRowViewModel> Results { get; } = [];

    public IRelayCommand OpenCompareCommand { get; }

    public CompareAllViewModel(IOverlayService overlay) {
        _overlay = overlay;
        OpenCompareCommand = new RelayCommand(OpenCompareOverlay);

        // Populate with the default reference string/frame count right away
        // so the page isn't empty the first time it's visited.
        InputParsing.TryParseReferenceString("7 0 1 2 0 3 0 4 2 3 0 3 2", out var reference, out _);
        InputParsing.TryParseFrameCount("3", out var frameCount, out _);
        Run(reference, frameCount);
    }

    private void OpenCompareOverlay() {
        _overlay.Open(new CompareAllInputViewModel(Run, _overlay.Close));
    }

    private void Run(List<int> reference, int frameCount) {
        var runs = AlgorithmCatalog.All
            .Select((algorithm, index) => (
                Result: algorithm.Run(reference, frameCount),
                Color: Palette[index % Palette.Length]
            ))
            .ToList();

        int bestFaults = runs.Count == 0 ? 0 : runs.Min(r => r.Result.PageFaults);

        Results.Clear();
        foreach (var run in runs.OrderBy(r => r.Result.PageFaults)) {
            Results.Add(new AlgorithmComparisonRowViewModel(run.Result, run.Result.PageFaults == bestFaults, run.Color));
        }

        ResultsSummaryLabel = $"Reference: {string.Join(' ', reference)}  ·  {frameCount} frames";
        HasResults = true;

        _overlay.Close();
    }

}
