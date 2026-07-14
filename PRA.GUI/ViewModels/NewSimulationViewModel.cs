using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PRA.Core.Interfaces;
using PRA.GUI.Models;
using PRA.GUI.Services;
using System;
using System.Collections.Generic;

namespace PRA.GUI.ViewModels;

public partial class NewSimulationViewModel : ViewModelBase {

    readonly private Action<List<int>, int, IPageReplacementAlgorithm> _onStart;
    readonly private Action _onCancel;

    [ObservableProperty] private string referenceStringInput = "7 0 1 2 0 3 0 4 2 3 0 3 2";
    [ObservableProperty] private string frameCountInput = "3";

    [ObservableProperty] private string? referenceStringError;
    [ObservableProperty] private string? frameCountError;

    public IReadOnlyList<IPageReplacementAlgorithm> Algorithms { get; } = AlgorithmCatalog.All;

    [ObservableProperty] private IPageReplacementAlgorithm selectedAlgorithm;

    public IRelayCommand StartCommand { get; }
    public IRelayCommand CancelCommand { get; }

    public NewSimulationViewModel(
        IPageReplacementAlgorithm currentAlgorithm,
        Action<List<int>, int, IPageReplacementAlgorithm> onStart,
        Action onCancel
    ) {
        _onStart = onStart;
        _onCancel = onCancel;
        selectedAlgorithm = currentAlgorithm;

        StartCommand = new RelayCommand(Start);
        CancelCommand = new RelayCommand(() => _onCancel());
    }

    private void Start() {
        bool referenceOk =
            InputParsing.TryParseReferenceString(ReferenceStringInput, out var reference, out string? referenceError);

        bool frameCountOk =
            InputParsing.TryParseFrameCount(FrameCountInput, out int frameCount, out string? frameCountErrorMessage);

        ReferenceStringError = referenceError;
        FrameCountError = frameCountErrorMessage;

        if (!referenceOk || !frameCountOk) return;

        _onStart(reference, frameCount, SelectedAlgorithm);
    }

}