using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PRA.GUI.Services;

namespace PRA.GUI.ViewModels;

/// <summary>
/// Backs the overlay that asks for a reference string and frame count before
/// running the Compare All page. Same shape as NewSimulationViewModel, minus
/// an algorithm picker — Compare All always runs every algorithm.
/// </summary>
public partial class CompareAllInputViewModel : ViewModelBase {

    private readonly Action<List<int>, int> _onRun;
    private readonly Action _onCancel;

    [ObservableProperty] private string referenceStringInput = "7 0 1 2 0 3 0 4 2 3 0 3 2";
    [ObservableProperty] private string frameCountInput = "3";

    [ObservableProperty] private string? referenceStringError;
    [ObservableProperty] private string? frameCountError;

    public IRelayCommand RunCommand { get; }
    public IRelayCommand CancelCommand { get; }

    public CompareAllInputViewModel(Action<List<int>, int> onRun, Action onCancel) {
        _onRun = onRun;
        _onCancel = onCancel;

        RunCommand = new RelayCommand(Run);
        CancelCommand = new RelayCommand(() => _onCancel());
    }

    private void Run() {
        bool referenceOk = InputParsing.TryParseReferenceString(ReferenceStringInput, out var reference, out var referenceError);
        bool frameCountOk = InputParsing.TryParseFrameCount(FrameCountInput, out var frameCount, out var frameCountErrorMessage);

        ReferenceStringError = referenceError;
        FrameCountError = frameCountErrorMessage;

        if (!referenceOk || !frameCountOk) return;

        _onRun(reference, frameCount);
    }

}
