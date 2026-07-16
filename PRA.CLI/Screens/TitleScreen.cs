using PRA.CLI.Components;
using Spectre.Console;

namespace PRA.CLI.Screens;

public class TitleScreen
{
    public void Show()
    {
        AnsiConsole.Clear();

        var figlet = Align.Center(
            new FigletText("PRA").Color(Theme.PrimaryColor));

        var subtitle = Align.Center(
            new Markup(Theme.Dim("Page Replacement Algorithm Simulator")));

        var content = new Rows(figlet, subtitle);

        var panel = new Panel(Align.Center(content, VerticalAlignment.Middle))
        {
            Header = new PanelHeader(Theme.Bold("SYSTEM BOOT"), Justify.Center),
            Border = Theme.Border,
            BorderStyle = Theme.BorderStyle,
            Padding = new Padding(4, 2, 4, 2)
        };

        AnsiConsole.Write(Align.Center(panel));

        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();

        var prompt = new Markup(Theme.Dim("Press any key to continue..."));
        AnsiConsole.Write(Align.Center(prompt));

        Console.ReadKey(true);
    }
}