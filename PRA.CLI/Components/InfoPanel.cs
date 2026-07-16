using PRA.Core.Models;
using Spectre.Console;

namespace PRA.CLI.Components;

public static class InfoPanel
{
    public static Panel Build(SimulationResult result, int currentStep)
    {
        var step = result.Steps[currentStep];

        var grid = new Grid();
        grid.AddColumn(new GridColumn().PadTop(0).PadBottom(1));

        grid.AddRow(new Markup(Theme.Bold(result.AlgorithmName.ToUpper())).Centered());
        grid.AddRow(new Markup($"Step {Theme.Warn($"{currentStep + 1}/{result.Steps.Count}")}").Centered());
        grid.AddRow(new Markup($"Page [white]{step.CurrentPage}[/]").Centered());

        grid.AddRow(new Markup(
            step.IsPageFault ? Theme.Fault("FAULT") : Theme.Hit("HIT")).Centered());

        grid.AddRow(new Markup(
            step.ReplacedPage is { } r
                ? $"replaced {Theme.Warn(r.ToString())}"
                : $"replaced {Theme.Dim("nothing")}"
        ).Centered());

        return new Panel(Align.Center(grid, VerticalAlignment.Middle))
        {
            Header = new PanelHeader(Theme.Bold("SIMULATION"), Justify.Center),
            Border = Theme.Border,
            BorderStyle = Theme.BorderStyle,
            Expand = true,
            Padding = new Padding(2, 1, 2, 1)
        };
    }
}