using Spectre.Console;

namespace PRA.CLI.Components;

public static class Header {

    public static void Draw() {
        AnsiConsole.Clear();

        var panel = new Panel(
            Align.Center(
                new Markup("[bold cornflowerblue]PAGE REPLACEMENT ALGORITHM SIMULATOR[/]"))
        ) {
            Border = BoxBorder.Double,
            Padding = new Padding(1, 0, 1, 0),
            Expand = true
        };

        AnsiConsole.Write(panel);
        AnsiConsole.WriteLine();
    }

}