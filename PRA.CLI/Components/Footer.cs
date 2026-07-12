using Spectre.Console;

namespace PRA.CLI.Components;

public static class Footer {

    public static void Draw() {
        AnsiConsole.Write(new Rule());

        AnsiConsole.MarkupLine(
            "[grey]← Previous[/]    " +
            "[grey]→ Next[/]    " +
            "[grey]Home[/]    " +
            "[grey]End[/]    " +
            "[red]Esc Exit[/]");
    }

}