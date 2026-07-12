using PRA.Core.Models;
using Spectre.Console;

namespace PRA.CLI.Components;

public static class StatisticsPanel {

    // StatisticsPanel.cs
    public static Panel Build(SimulationResult result, int currentStep) {
        int faults = result.Steps.Take(currentStep + 1).Count(s => s.IsPageFault);
        int hits = currentStep + 1 - faults;
        double ratio = (double)hits / (currentStep + 1);

        var grid = new Grid().AddColumn().AddColumn();
        grid.AddRow("[green]Hits[/]", hits.ToString());
        grid.AddRow("[red]Faults[/]", faults.ToString());
        grid.AddRow("[yellow]Ratio[/]", $"{ratio:P0}");

        var chart = new BreakdownChart()
            .Width(22)
            .AddItem("Hits", hits, Color.Green)
            .AddItem("Faults", faults, Color.Red);

        var content = new Rows(
            Align.Center(grid),
            Align.Center(chart)
        );

        return new Panel(Align.Center(content, VerticalAlignment.Middle)) {
            Header = new PanelHeader("STATISTICS", Justify.Center),
            Border = BoxBorder.Rounded,
            Expand = true,
            Padding = new Padding(2, 0, 2, 1)
        };
    }

}