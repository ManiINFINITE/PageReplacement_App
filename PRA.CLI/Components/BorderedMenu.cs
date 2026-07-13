using Spectre.Console;
using Spectre.Console.Rendering;

namespace PRA.CLI.Components;

public static class BorderedMenu {

    public static string Show(string title, IReadOnlyList<string> options) {
        int selected = 0;

        while (true) {
            AnsiConsole.Clear();
            Header.Draw();
            AnsiConsole.WriteLine();

            var rows = new List<IRenderable>();

            for (int i = 0; i < options.Count; i++) {
                string label = options[i];

                rows.Add(i == selected
                    ? new Markup($"[{Theme.CurrentTag}] ► {label,-30} [/]")
                    : new Markup($"{Theme.Dim($"   {label,-30} ")}"));
            }

            var panel = new Panel(Align.Center(new Rows(rows), VerticalAlignment.Middle)) {
                Header = new PanelHeader(Theme.Bold(title.ToUpper()), Justify.Center),
                Border = Theme.Border,
                BorderStyle = Theme.BorderStyle,
                Padding = new Padding(4, 2, 4, 2)
            };

            AnsiConsole.Write(Align.Center(panel));

            AnsiConsole.WriteLine();

            AnsiConsole.Write(Align.Center(new Markup(
                $"{Theme.Dim("↑/↓")} Move   {Theme.Dim("Enter")} Select")));

            var key = Console.ReadKey(true);

            switch (key.Key) {
                case ConsoleKey.UpArrow:
                    selected = (selected - 1 + options.Count) % options.Count;
                    break;
                case ConsoleKey.DownArrow:
                    selected = (selected + 1) % options.Count;
                    break;
                case ConsoleKey.Enter:
                    return options[selected];
            }
        }
    }

}