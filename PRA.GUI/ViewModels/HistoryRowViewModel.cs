using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace PRA.GUI.ViewModels;

public partial class HistoryCellViewModel(string value, bool isFault) : ObservableObject {

    public string Value { get; } = value;
    public bool IsFault { get; } = isFault;

    [ObservableProperty] private bool isCurrentStep;

}

public class HistoryRowViewModel(string label, List<HistoryCellViewModel> cells) {

    public string Label => label;
    public List<HistoryCellViewModel> Cells => cells;

    public void SetCurrentStep(int step) {
        for (int i = 0; i < cells.Count; i++)
            cells[i].IsCurrentStep = i == step;
    }

}