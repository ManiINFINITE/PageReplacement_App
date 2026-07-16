using Spectre.Console;
using Spectre.Console.Rendering;

namespace PRA.CLI.Components;

public static class Footer
{
    public static IRenderable Build()
    {
        var keys = new Markup(
            $"{Theme.Dim("←/→")} Step   " +
            $"{Theme.Dim("Home/End")} Jump   " +
            $"{Theme.Dim("T")} Toggle history   " +
            $"{Theme.Dim("E")} Export   " +
            $"{Theme.Accent("Esc")} Back");

        return new Rows(
            new Rule().RuleStyle(Theme.BorderStyle),
            Align.Center(keys)
        );
    }

    public static void Draw()
    {
        AnsiConsole.Write(new Rule().RuleStyle(Theme.BorderStyle));

        AnsiConsole.MarkupLine(
            $"{Theme.Dim("← Previous")}    {Theme.Dim("→ Next")}    " +
            $"{Theme.Dim("Home")}    {Theme.Dim("End")}    {Theme.Accent("Esc Exit")}");
    }
}