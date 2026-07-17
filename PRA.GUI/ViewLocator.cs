using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using PRA.GUI.ViewModels;

namespace PRA.GUI;

/// <summary>
/// Given a view model, returns the corresponding view if possible.
/// </summary>
[RequiresUnreferencedCode(
    "Default implementation of ViewLocator involves reflection which may be trimmed away.",
    Url = "https://docs.avaloniaui.net/docs/concepts/view-locator")]
public class ViewLocator : IDataTemplate
{
    // Map ViewModels that should use a different View than the convention
    private static readonly Dictionary<Type, Type> ViewModelViewMap = new()
    {
        // Both Export ViewModels use the same View
        { typeof(ExportComparisonViewModel), typeof(Views.ExportInfoView) },
        // Add more mappings here if needed in the future
        // { typeof(SomeOtherViewModel), typeof(SomeOtherView) },
    };

    public Control? Build(object? param)
    {
        if (param is null)
            return null;

        var viewModelType = param.GetType();

        // 1. Check if we have a custom mapping for this ViewModel
        if (ViewModelViewMap.TryGetValue(viewModelType, out var viewType))
        {
            return (Control)Activator.CreateInstance(viewType)!;
        }

        // 2. Fallback to convention-based naming
        string name = viewModelType.FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        viewType = Type.GetType(name);

        if (viewType != null)
        {
            return (Control)Activator.CreateInstance(viewType)!;
        }

        // 3. If all else fails, show a not found message
        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}