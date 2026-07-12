using PRA.Core.Models;
using Spectre.Console;

namespace PRA.CLI.Components;

public static class InfoPanel {

    public static Panel Build(SimulationResult result, int currentStep) {
        var step = result.Steps[currentStep];

        var grid = new Grid();
        grid.AddColumn(new GridColumn().PadTop(0).PadBottom(1));

        grid.AddRow(new Markup($"[bold cornflowerblue]{result.AlgorithmName}[/]").Centered());
        grid.AddRow(new Markup($"Step [yellow]{currentStep + 1}/{result.Steps.Count}[/]").Centered());
        grid.AddRow(new Markup($"Page [white]{step.CurrentPage}[/]").Centered());
        grid.AddRow(new Markup(step.IsPageFault ? "[red]FAULT[/]" : "[green]HIT[/]").Centered());

        grid.AddRow(new Markup(
            step.ReplacedPage is { } r
                ? $"replaced [orange1]{r}[/]"
                : "replaced [grey]nothing[/]"
        ).Centered());

        return new Panel(Align.Center(grid, VerticalAlignment.Middle)) {
            Header = new PanelHeader("SIMULATION", Justify.Center),
            Border = BoxBorder.Rounded,
            Expand = true,
            Padding = new Padding(2, 1, 2, 1)
        };
    }

}