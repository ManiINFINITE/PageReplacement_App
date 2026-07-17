using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PRA.Core.Interfaces;
using PRA.GUI.Models;
using PRA.GUI.Services;
using System.Collections.Generic;

namespace PRA.GUI.ViewModels;

/// <summary>
///     Runs two chosen algorithms against the same reference string and frame
///     count, then shows each as a full single-simulation panel side by side.
///     Input is collected via an overlay (see CompareTwoInputViewModel).
/// </summary>
public partial class CompareTwoViewModel : ViewModelBase
{
    readonly private IOverlayService _overlay;

    [ObservableProperty] private bool hasResults;
    [ObservableProperty] private string resultsSummaryLabel = "";

    [ObservableProperty] private DashboardViewModel? left;
    [ObservableProperty] private DashboardViewModel? right;

    public IRelayCommand OpenCompareCommand { get; }

    public CompareTwoViewModel(IOverlayService overlay)
    {
        _overlay = overlay;
        OpenCompareCommand = new RelayCommand(OpenCompareOverlay);

        // Populate with sensible defaults right away so the page isn't empty.
        var algorithms = AlgorithmCatalog.All;
        var algorithmA = algorithms[0];
        var algorithmB = algorithms.Count > 1 ? algorithms[1] : algorithms[0];

        InputParsing.TryParseReferenceString("7 0 1 2 0 3 0 4 2 3 0 3 2", out var reference, out _);
        InputParsing.TryParseFrameCount("3", out int frameCount, out _);
        Run(reference, frameCount, algorithmA, algorithmB);
    }

    private void OpenCompareOverlay()
    {
        _overlay.Open(new CompareTwoInputViewModel(Run, _overlay.Close));
    }

    private void Run(
        List<int> reference,
        int frameCount,
        IPageReplacementAlgorithm algorithmA,
        IPageReplacementAlgorithm algorithmB
    )
    {
        var resultA = algorithmA.Run(reference, frameCount);
        var resultB = algorithmB.Run(reference, frameCount);

        Left = new DashboardViewModel(resultA, reference);
        Right = new DashboardViewModel(resultB, reference);

        ResultsSummaryLabel = $"Reference: {string.Join(' ', reference)}  ·  {frameCount} frames";
        HasResults = true;

        _overlay.Close();
    }

    [RelayCommand]
    private void BothNextStep()
    {
        Right!.NextStepCommand.Execute(null);
        Left!.NextStepCommand.Execute(null);
    }

    [RelayCommand]
    private void BothPreviousStep()
    {
        Right!.PreviousStepCommand.Execute(null);
        Left!.PreviousStepCommand.Execute(null);
    }

    [RelayCommand]
    private void BothToStart()
    {
        Right!.JumpToStartCommand.Execute(null);
        Left!.JumpToStartCommand.Execute(null);
    }

    [RelayCommand]
    private void BothToEnd()
    {
        Right!.JumpToEndCommand.Execute(null);
        Left!.JumpToEndCommand.Execute(null);
    }
}