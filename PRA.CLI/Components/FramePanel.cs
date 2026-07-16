using PRA.Core.Models;
using Spectre.Console;

namespace PRA.CLI.Components;

public static class FramePanel
{
    public static Panel Build(SimulationStep step)
    {
        var table = new Table()
            .Border(TableBorder.Simple)
            .BorderStyle(Theme.BorderStyle)
            .Expand();

        for (int i = 0; i < step.Frames.Count; i++)
        {
            table.AddColumn(new TableColumn(Theme.Dim($"F{i}")).Centered());
        }

        string[] valueRow = step.Frames.Select((f, i) =>
        {
            string val = f?.ToString() ?? "-";
            if (f == step.ReplacedPage) return $"[{Theme.ReplacedTag}] {val} [/]";
            if (f == step.CurrentPage && !step.IsPageFault) return Theme.Hit(val);
            if (f == step.CurrentPage && step.IsPageFault) return Theme.Fault(val);

            return Theme.Accent(val);
        }).ToArray();

        table.AddRow(valueRow);

        if (step.ReferenceBits is not null)
        {
            string[] bits = step.ReferenceBits
                .Select((b, i) =>
                    (i == step.ClockPointer ? Theme.Warn("➤") : " ") +
                    (b ? Theme.Accent("●") : Theme.Dim("○")))
                .ToArray();

            table.AddRow(bits);
        }

        return new Panel(Align.Center(table))
        {
            Header = new PanelHeader(Theme.Bold("FRAMES"), Justify.Center),
            Border = Theme.Border,
            BorderStyle = Theme.BorderStyle,
            Expand = true,
            Padding = new Padding(2, 1, 2, 1)
        };
    }
}