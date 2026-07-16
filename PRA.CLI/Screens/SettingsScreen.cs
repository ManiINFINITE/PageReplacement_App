using PRA.CLI.Components;
using PRA.CLI.Services;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace PRA.CLI.Screens;

public class SettingsScreen
{
    public void Show()
    {
        int selected = Array.FindIndex(Theme.AvailableColors, c => c.Name == Theme.PrimaryName);
        if (selected < 0) selected = 0;

        while (true)
        {
            AnsiConsole.Clear();
            Header.Draw();
            AnsiConsole.WriteLine();

            var rows = new List<IRenderable>();

            for (int i = 0; i < Theme.AvailableColors.Length; i++)
            {
                (string name, string swatch) = Theme.AvailableColors[i];
                string swatchBlock = $"[{swatch}]■■■[/]";
                string label = $"{swatchBlock} {name,-16}";

                rows.Add(i == selected
                    ? new Markup($"[black on {swatch}] ► {label} [/]")
                    : new Markup($"   {label} "));
            }

            var panel = new Panel(Align.Center(new Rows(rows), VerticalAlignment.Middle))
            {
                Header = new PanelHeader(Theme.Bold("SETTINGS — ACCENT COLOR"), Justify.Center),
                Border = Theme.Border,
                BorderStyle = Theme.BorderStyle,
                Padding = new Padding(4, 2, 4, 2)
            };

            AnsiConsole.Write(Align.Center(panel));

            AnsiConsole.WriteLine();

            AnsiConsole.Write(Align.Center(new Markup(
                $"{Theme.Dim("↑/↓")} Move   {Theme.Dim("Enter")} Apply   {Theme.Dim("Esc")} Back")));

            var key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    selected = (selected - 1 + Theme.AvailableColors.Length) % Theme.AvailableColors.Length;
                    break;
                case ConsoleKey.DownArrow:
                    selected = (selected + 1) % Theme.AvailableColors.Length;
                    break;
                case ConsoleKey.Enter:
                    Theme.SetPrimary(Theme.AvailableColors[selected].Name);
                    SettingsService.SavePrimaryColor(Theme.AvailableColors[selected].Name);
                    return;
                case ConsoleKey.Escape:
                    return;
            }
        }
    }
}