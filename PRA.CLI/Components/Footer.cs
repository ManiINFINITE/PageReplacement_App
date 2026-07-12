using Spectre.Console;
using Spectre.Console.Rendering;

namespace PRA.CLI.Components;

public static class Footer {

    public static IRenderable Build() {
        var keys = new Markup(
            "[grey]←/→[/] Step   " +
            "[grey]Home/End[/] Jump   " +
            "[grey]T[/] Toggle history   " +
            "[grey]E[/] Export   " +
            "[grey]Esc[/] Back");

        return new Rows(
            new Rule().RuleStyle(Theme.Muted),
            Align.Center(keys)
        );
    }

    // Kept for screens outside the Layout (menus, compare table) that just want to print inline.
    public static void Draw() {
        AnsiConsole.Write(new Rule().RuleStyle(Theme.Muted));

        AnsiConsole.MarkupLine(
            "[grey]← Previous[/]    [grey]→ Next[/]    [grey]Home[/]    [grey]End[/]    [red]Esc Exit[/]");
    }

}