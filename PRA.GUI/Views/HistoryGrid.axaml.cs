using Avalonia;
using Avalonia.Controls;
using PRA.GUI.ViewModels;
using System.Collections.Generic;

namespace PRA.GUI.Views;

public partial class HistoryGrid : UserControl
{
    public readonly static StyledProperty<IEnumerable<HistoryRowViewModel>?> RowsProperty =
        AvaloniaProperty.Register<HistoryGrid, IEnumerable<HistoryRowViewModel>?>(nameof(Rows));

    public IEnumerable<HistoryRowViewModel>? Rows
    {
        get => GetValue(RowsProperty);
        set => SetValue(RowsProperty, value);
    }

    public HistoryGrid()
    {
        InitializeComponent();
    }
}