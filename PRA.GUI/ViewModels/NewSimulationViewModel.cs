using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PRA.Core.Interfaces;
using PRA.GUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PRA.GUI.ViewModels;

/// <summary>
///     Backs the "New Simulation" overlay. Collects a reference string and frame
///     count, validates them, then hands the parsed values off via the onStart
///     callback so the shell decides what to do with them.
/// </summary>
public partial class NewSimulationViewModel : ViewModelBase {

    readonly private Action<List<int>, int, IPageReplacementAlgorithm> _onStart;
    readonly private Action _onCancel;

    public IReadOnlyList<IPageReplacementAlgorithm> Algorithms { get; } = AlgorithmCatalog.All;

    [ObservableProperty] private IPageReplacementAlgorithm selectedAlgorithm;

    [ObservableProperty] private string referenceStringInput = "7 0 1 2 0 3 0 4 2 3 0 3 2";
    [ObservableProperty] private string frameCountInput = "3";

    [ObservableProperty] private string? referenceStringError;
    [ObservableProperty] private string? frameCountError;

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
        var reference = ParseReferenceString(ReferenceStringInput);
        int? frameCount = ParseFrameCount(FrameCountInput);

        if (reference is null || frameCount is null) return;
        _onStart(reference, frameCount.Value, SelectedAlgorithm);
    }

    private List<int>? ParseReferenceString(string input) {
        ReferenceStringError = null;

        var tokens = Regex.Split(input.Trim(), @"[\s,]+")
            .Where(t => t.Length > 0)
            .ToList();

        if (tokens.Count == 0) {
            ReferenceStringError = "Enter at least one page number.";
            return null;
        }

        var values = new List<int>();

        foreach (string token in tokens) {
            if (!int.TryParse(token, out int value)) {
                ReferenceStringError = $"'{token}' isn't a valid page number.";
                return null;
            }

            values.Add(value);
        }

        return values;
    }

    private int? ParseFrameCount(string input) {
        FrameCountError = null;

        if (!int.TryParse(input.Trim(), out int frameCount) || frameCount <= 0) {
            FrameCountError = "Frame count must be a positive whole number.";
            return null;
        }

        return frameCount;
    }

}