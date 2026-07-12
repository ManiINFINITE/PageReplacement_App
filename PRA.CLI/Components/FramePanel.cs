using PRA.Core.Models;
using Spectre.Console;

namespace PRA.CLI.Components;

public static class FramePanel {

    public static Panel Build(SimulationStep step) {
        var table = new Table().Border(TableBorder.Simple).Expand();

        for (int i = 0; i < step.Frames.Count; i++)
            table.AddColumn(new TableColumn($"[grey]F{i}[/]").Centered());

        var valueRow = step.Frames.Select((f, i) => {
            string val = f?.ToString() ?? "-";
            if (f == step.ReplacedPage) return $"[{Theme.ReplacedTag}] {val} [/]";
            if (f == step.CurrentPage && !step.IsPageFault) return $"[{Theme.HitTag}]{val}[/]";
            if (f == step.CurrentPage && step.IsPageFault) return $"[{Theme.FaultTag}]{val}[/]";

            return val;
        }).ToArray();

        table.AddRow(valueRow);

        // Clock: show reference bits + pointer under the frames
        if (step.ReferenceBits is not null) {
            var bits = step.ReferenceBits
                .Select((b, i) => (i == step.ClockPointer ? "[yellow]➤[/]" : " ") + (b ? "●" : "○"))
                .ToArray();

            table.AddRow(bits);
        }

        // FramePanel.cs — center the table within the panel too
        return new Panel(Align.Center(table)) {
            Header = new PanelHeader("FRAMES", Justify.Center),
            Border = BoxBorder.Rounded,
            Expand = true,
            Padding = new Padding(2, 0, 2, 1)
        };
    }

}