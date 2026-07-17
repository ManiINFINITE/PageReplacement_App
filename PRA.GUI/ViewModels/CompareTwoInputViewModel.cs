using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PRA.Core.Interfaces;
using PRA.GUI.Models;
using PRA.GUI.Services;
using System;
using System.Collections.Generic;

namespace PRA.GUI.ViewModels;

/// <summary>
///     Backs the overlay that asks for a reference string, frame count, and two
///     different algorithms before running the Compare Two page.
/// </summary>
public partial class CompareTwoInputViewModel : ViewModelBase
{
    readonly private Action<List<int>, int, IPageReplacementAlgorithm, IPageReplacementAlgorithm> _onRun;
    readonly private Action _onCancel;

    [ObservableProperty] private string referenceStringInput = "7 0 1 2 0 3 0 4 2 3 0 3 2";
    [ObservableProperty] private string frameCountInput = "3";

    [ObservableProperty] private string? referenceStringError;
    [ObservableProperty] private string? frameCountError;
    [ObservableProperty] private string? algorithmError;

    public IReadOnlyList<IPageReplacementAlgorithm> Algorithms { get; } = AlgorithmCatalog.All;

    [ObservableProperty] private IPageReplacementAlgorithm algorithmA;
    [ObservableProperty] private IPageReplacementAlgorithm algorithmB;

    public IRelayCommand RunCommand { get; }
    public IRelayCommand CancelCommand { get; }

    public CompareTwoInputViewModel(
        Action<List<int>, int, IPageReplacementAlgorithm, IPageReplacementAlgorithm> onRun,
        Action onCancel
    )
    {
        _onRun = onRun;
        _onCancel = onCancel;

        algorithmA = Algorithms[0];
        algorithmB = Algorithms.Count > 1 ? Algorithms[1] : Algorithms[0];

        RunCommand = new RelayCommand(Run);
        CancelCommand = new RelayCommand(() => _onCancel());
    }

    private void Run()
    {
        bool referenceOk =
            InputParsing.TryParseReferenceString(ReferenceStringInput, out var reference, out string? referenceError);

        bool frameCountOk =
            InputParsing.TryParseFrameCount(FrameCountInput, out int frameCount, out string? frameCountErrorMessage);

        ReferenceStringError = referenceError;
        FrameCountError = frameCountErrorMessage;

        AlgorithmError = AlgorithmA.Name == AlgorithmB.Name
            ? "Pick two different algorithms."
            : null;

        if (!referenceOk || !frameCountOk || AlgorithmError is not null) return;

        _onRun(reference, frameCount, AlgorithmA, AlgorithmB);
    }
}