using CommunityToolkit.Mvvm.Input;
using PRA.Core.Models;
using PRA.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PRA.GUI.ViewModels;

public partial class ExportComparisonViewModel : ExportViewModelBase
{
    readonly private IReadOnlyList<SimulationResult> _results;
    readonly private IReadOnlyList<int> _referenceString;

    public ExportComparisonViewModel(
        IReadOnlyList<SimulationResult> results,
        IReadOnlyList<int> referenceString,
        Action closeAction
    )
        : base(closeAction)
    {
        _results = results;
        _referenceString = referenceString;

        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string firstAlgo = results.FirstOrDefault()?.AlgorithmName ?? "Comparison";
        string safeName = firstAlgo.Replace(" ", "_").Replace("/", "_");
        FileName = $"Comparison_{safeName}_{timestamp}";
    }

    // NO [RelayCommand] attribute here - it's inherited from the base class
    protected override void Export()
    {
        if (string.IsNullOrWhiteSpace(FileName))
        {
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            FileName = $"comparison_{timestamp}";
        }

        string content;
        string fullFileName;

        switch (SelectedExportMode)
        {
            case "CSV":
                content = ExportResult.ComparisonToCsv(_results, _referenceString);
                fullFileName = FileName.EndsWith(".csv") ? FileName : $"{FileName}.csv";
                break;
            case "Markdown":
                content = ExportResult.ComparisonToMarkdown(_results, _referenceString);
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