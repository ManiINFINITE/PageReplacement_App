using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PRA.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PRA.GUI.ViewModels;

public partial class DashboardViewModel : ViewModelBase {

    private readonly SimulationResult _result;
    private readonly IReadOnlyList<int> _referenceString;

    [ObservableProperty] private int currentStep;

    public ObservableCollection<ReferenceTokenViewModel> ReferenceTokens { get; } = [];
    public ObservableCollection<FrameViewModel> Frames { get; } = [];
    public ObservableCollection<HistoryRowViewModel> HistoryRows { get; } = [];

    public string AlgorithmName => _result.AlgorithmName;
    public string ReferenceStringLabel => "Reference: " + string.Join(' ', _referenceString);
    public string StepLabel => $"{CurrentStep + 1}/{_result.Steps.Count}";
    public string CurrentPageLabel => _result.Steps[CurrentStep].CurrentPage.ToString();
    public bool IsHitStep => !_result.Steps[CurrentStep].IsPageFault;
    public bool IsFaultStep => _result.Steps[CurrentStep].IsPageFault;
    public string ResultLabel => IsFaultStep ? "FAULT" : "HIT";
    public int Hits => _result.Steps.Take(CurrentStep + 1).Count(s => !s.IsPageFault);
    public int Faults => _result.Steps.Take(CurrentStep + 1).Count(s => s.IsPageFault);
    public string HitRatioLabel => $"{(double)Hits / (CurrentStep + 1):P0}";

    public IRelayCommand NextStepCommand { get; }
    public IRelayCommand PreviousStepCommand { get; }
    public IRelayCommand JumpToStartCommand { get; }
    public IRelayCommand JumpToEndCommand { get; }

    public DashboardViewModel(SimulationResult result, IReadOnlyList<int> referenceString) {
        _result = result;
        _referenceString = referenceString;

        NextStepCommand = new RelayCommand(() =>
            CurrentStep = Math.Min(CurrentStep + 1, _result.Steps.Count - 1));
        PreviousStepCommand = new RelayCommand(() =>
            CurrentStep = Math.Max(CurrentStep - 1, 0));
        JumpToStartCommand = new RelayCommand(() => CurrentStep = 0);
        JumpToEndCommand = new RelayCommand(() => CurrentStep = _result.Steps.Count - 1);

        BuildHistoryRows();
        Refresh();
    }

    partial void OnCurrentStepChanged(int value) => Refresh();

    private void Refresh() {
        RefreshReferenceTokens();
        RefreshFrames();

        OnPropertyChanged(nameof(StepLabel));
        OnPropertyChanged(nameof(CurrentPageLabel));
        OnPropertyChanged(nameof(IsHitStep));
        OnPropertyChanged(nameof(IsFaultStep));
        OnPropertyChanged(nameof(ResultLabel));
        OnPropertyChanged(nameof(Hits));
        OnPropertyChanged(nameof(Faults));
        OnPropertyChanged(nameof(HitRatioLabel));

        foreach (var row in HistoryRows) row.SetCurrentStep(CurrentStep);
    }

    private void RefreshReferenceTokens() {
        ReferenceTokens.Clear();
        for (int i = 0; i < _referenceString.Count; i++) {
            bool isCurrent = i == CurrentStep;
            bool isPast = i < CurrentStep;
            bool isFault = isPast && _result.Steps[i].IsPageFault;
            bool isHit = isPast && !_result.Steps[i].IsPageFault;
            ReferenceTokens.Add(new ReferenceTokenViewModel(_referenceString[i], isCurrent, isHit, isFault));
        }
    }

    private void RefreshFrames() {
        Frames.Clear();
        var step = _result.Steps[CurrentStep];

        for (int i = 0; i < step.Frames.Count; i++) {
            var val = step.Frames[i];
            bool isCurrentPage = val.HasValue && val.Value == step.CurrentPage;
            bool isReplaced = isCurrentPage && step.IsPageFault && step.ReplacedPage.HasValue;
            bool isFault = isCurrentPage && step.IsPageFault && !step.ReplacedPage.HasValue;
            bool isHit = isCurrentPage && !step.IsPageFault;

            Frames.Add(new FrameViewModel($"F{i}", val?.ToString() ?? "-", isHit, isFault, isReplaced));
        }
    }

    private void BuildHistoryRows() {
        int frameCount = _result.Steps[0].Frames.Count;

        for (int f = 0; f < frameCount; f++) {
            var cells = new List<HistoryCellViewModel>();
            for (int s = 0; s < _result.Steps.Count; s++) {
                var val = _result.Steps[s].Frames[f];
                bool isFaultCell = _result.Steps[s].IsPageFault && val == _result.Steps[s].CurrentPage;
                cells.Add(new HistoryCellViewModel(val?.ToString() ?? "·", isFaultCell));
            }
            HistoryRows.Add(new HistoryRowViewModel($"F{f}", cells));
        }
    }

}