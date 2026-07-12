using PRA.Core.Models;
using Spectre.Console;

namespace PRA.CLI.Components;

public static class HistoryTable {

    public static Panel Build(SimulationResult result, IReadOnlyList<int> referenceString, int currentStep) {
        int frameCount = result.Steps[0].Frames.Count;
        var table = new Table().Border(TableBorder.Minimal);

        table.AddColumn(new TableColumn("").Centered().Padding(1, 1, 1, 0));

        foreach (var page in referenceString)
            table.AddColumn(new TableColumn($"[grey]{page}[/]").Centered().Padding(1, 1, 1, 0));

        for (int f = 0; f < frameCount; f++) {
            var row = new List<string> { $"[grey]F{f}[/]" };

            for (int s = 0; s < result.Steps.Count; s++) {
                var val = result.Steps[s].Frames[f];
                string cell = val?.ToString() ?? "·";

                if (s == currentStep) cell = $"[{Theme.CurrentTag}]{cell}[/]";
                else if (result.Steps[s].IsPageFault && result.Steps[s].Frames[f] == result.Steps[s].CurrentPage)
                    cell = $"[red]{cell}[/]";

                row.Add(cell);
            }

            table.AddRow(row.ToArray());
        }

        return new Panel(Align.Center(table, VerticalAlignment.Middle)) {
            Header = new PanelHeader("HISTORY", Justify.Center),
            Border = BoxBorder.Rounded,
            Expand = true,
            Padding = new Padding(2, 0, 2, 1)
        };
    }

}