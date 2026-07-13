using PRA.Core.Models;
using Spectre.Console;

namespace PRA.CLI.Components;

public static class HistoryTable {

    public static Panel Build(SimulationResult result, IReadOnlyList<int> referenceString, int currentStep) {
        int frameCount = result.Steps[0].Frames.Count;

        var table = new Table()
            .Border(TableBorder.Minimal)
            .BorderStyle(Theme.BorderStyle);

        table.AddColumn(new TableColumn("").Centered().Padding(1, 1, 1, 0));

        foreach (var page in referenceString)
            table.AddColumn(new TableColumn(Theme.Dim(page.ToString())).Centered().Padding(1, 1, 1, 0));

        for (int f = 0; f < frameCount; f++) {
            var row = new List<string> { Theme.Dim($"F{f}") };

            for (int s = 0; s < result.Steps.Count; s++) {
                var val = result.Steps[s].Frames[f];
                string cell = val?.ToString() ?? "·";

                if (s == currentStep)
                    cell = $"[{Theme.CurrentTag}]{cell}[/]";
                else if (result.Steps[s].IsPageFault && result.Steps[s].Frames[f] == result.Steps[s].CurrentPage)
                    cell = Theme.Fault(cell);
                else
                    cell = Theme.Accent(cell);

                row.Add(cell);
            }

            table.AddRow(row.ToArray());
        }

        return new Panel(Align.Center(table, VerticalAlignment.Middle)) {
            Header = new PanelHeader(Theme.Bold("HISTORY"), Justify.Center),
            Border = Theme.Border,
            BorderStyle = Theme.BorderStyle,
            Expand = true,
            Padding = new Padding(1, 1, 1, 1)
        };
    }

}