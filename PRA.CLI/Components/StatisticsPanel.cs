using PRA.Core.Models;
using Spectre.Console;

namespace PRA.CLI.Components;

public static class StatisticsPanel {

    public static Panel Build(SimulationResult result, int currentStep) {
        int faults = result.Steps
            .Take(currentStep + 1)
            .Count(s => s.IsPageFault);

        int hits = currentStep + 1 - faults;

        var grid = new Grid();

        grid.AddColumn();
        grid.AddColumn();

        grid.AddRow("[green]Hits[/]", hits.ToString());
        grid.AddRow("[red]Faults[/]", faults.ToString());

        grid.AddEmptyRow();

        grid.AddRow(
            "[yellow]Progress[/]",
            $"{currentStep + 1}/{result.Steps.Count}");

        return new Panel(grid) {
            Header = new PanelHeader("Statistics"),
            Border = BoxBorder.Rounded
        };
    }

}