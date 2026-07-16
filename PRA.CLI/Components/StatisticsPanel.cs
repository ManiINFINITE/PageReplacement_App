using PRA.Core.Models;
using Spectre.Console;

namespace PRA.CLI.Components;

public static class StatisticsPanel
{
    public static Panel Build(SimulationResult result, int currentStep)
    {
        int faults = result.Steps.Take(currentStep + 1).Count(s => s.IsPageFault);
        int hits = currentStep + 1 - faults;
        double ratio = (double)hits / (currentStep + 1);

        var grid = new Grid().AddColumn().AddColumn();
        grid.AddRow(Theme.Hit("Hits"), hits.ToString());
        grid.AddRow(Theme.Fault("Faults"), faults.ToString());
        grid.AddRow(Theme.Warn("Ratio"), $"{ratio:P0}");

        var chart = new BreakdownChart()
            .Width(22)
            .AddItem("Hits", hits, Theme.HitColor)
            .AddItem("Faults", faults, Theme.FaultColor);

        var content = new Rows(
            Align.Center(grid),
            Align.Center(chart)
        );

        return new Panel(Align.Center(content, VerticalAlignment.Middle))
        {
            Header = new PanelHeader(Theme.Bold("STATISTICS"), Justify.Center),
            Border = Theme.Border,
            BorderStyle = Theme.BorderStyle,
            Expand = true,
            Padding = new Padding(2, 1, 2, 1)
        };
    }
}