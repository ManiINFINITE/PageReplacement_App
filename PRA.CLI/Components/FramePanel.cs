using PRA.Core.Models;
using Spectre.Console;

namespace PRA.CLI.Components;

public static class FramePanel {

    public static Panel Build(SimulationStep step) {
        var table = new Table()
            .Border(TableBorder.None)
            .HideHeaders();

        table.AddColumn("");

        foreach (var frame in step.Frames) {
            table.AddRow(frame?.ToString() ?? "-");
        }

        return new Panel(table) {
            Header = new PanelHeader("Frames"),
            Border = BoxBorder.Rounded
        };
    }

}