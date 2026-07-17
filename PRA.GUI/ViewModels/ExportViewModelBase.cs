using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;

namespace PRA.GUI.ViewModels;

public abstract partial class ExportViewModelBase(Action closeAction) : ViewModelBase
{
    protected readonly Action _closeAction = closeAction;

    [ObservableProperty] private string _fileName = string.Empty;

    [ObservableProperty] private string _selectedExportMode = "Markdown";

    public IReadOnlyList<string> ExportModes { get; } = ["CSV", "Markdown"];

    // Base class defines the command - NO [RelayCommand] on abstract methods
    [RelayCommand]
    protected abstract void Export();

    [RelayCommand]
    protected virtual void Cancel()
    {
        _closeAction?.Invoke();
    }
}