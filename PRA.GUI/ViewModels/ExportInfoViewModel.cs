using CommunityToolkit.Mvvm.Input;
using PRA.Core.Models;
using PRA.Core.Utilities;
using System;
using System.Collections.Generic;

namespace PRA.GUI.ViewModels;

public partial class ExportInfoViewModel : ExportViewModelBase
{
    readonly private SimulationResult _result;
    readonly private IReadOnlyList<int> _referenceString;

    public ExportInfoViewModel(
        SimulationResult result,
        IReadOnlyList<int> referenceString,
        Action closeAction
    )
        : base(closeAction)
    {
        _result = result;
        _referenceString = referenceString;

        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string safeName = result.AlgorithmName.Replace(" ", "_").Replace("/", "_");
        FileName = $"{safeName}_{timestamp}";
    }

    // NO [RelayCommand] attribute here - it's inherited from the base class
    protected override void Export()
    {
        if (string.IsNullOrWhiteSpace(FileName))
        {
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            FileName = $"export_{timestamp}";
        }

        string content;
        string fullFileName;

        switch (SelectedExportMode)
        {
            case "CSV":
                content = ExportResult.ToCsv(_result, _referenceString);
                fullFileName = FileName.EndsWith(".csv") ? FileName : $"{FileName}.csv";
                break;
            case "Markdown":
                content = ExportResult.ToMarkdown(_result, _referenceString);
                fullFileName = FileName.EndsWith(".md") ? FileName : $"{FileName}.md";
                break;
            default:
                return;
        }

        string path = ExportResult.Save(content, fullFileName);
        Console.WriteLine($"{fullFileName} saved to {path}");
        _closeAction?.Invoke();
    }
}