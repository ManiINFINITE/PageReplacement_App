using Spectre.Console;

namespace PRA.CLI.Components;

public static class BorderedInput
{
    public static string Show(
        string label,
        string? placeholder = null,
        Func<string, bool>? validate = null,
        string? errorMessage = null
    )
    {
        string input = "";
        string? error = null;

        while (true)
        {
            AnsiConsole.Clear();
            Header.Draw();
            AnsiConsole.WriteLine();

            string display = input.Length > 0
                ? Markup.Escape(input)
                : Theme.Dim(Markup.Escape(placeholder ?? ""));

            var content = new Markup($"{Theme.Accent(">")} {display}[{Theme.PrimaryName} blink]_[/]");

            var panel = new Panel(content)
            {
                Header = new PanelHeader(Theme.Bold(label.ToUpper()), Justify.Center),
                Border = Theme.Border,
                BorderStyle = Theme.BorderStyle,
                Padding = new Padding(2, 1, 2, 1)
            };

            AnsiConsole.Write(Align.Center(panel));

            if (error is not null)
            {
                AnsiConsole.WriteLine();
                AnsiConsole.Write(Align.Center(new Markup(Theme.Fault(error))));
            }

            AnsiConsole.WriteLine();

            AnsiConsole.Write(Align.Center(new Markup(
                $"{Theme.Dim("Enter")} Confirm   {Theme.Dim("Backspace")} Delete")));

            var key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Enter)
            {
                if (input.Length == 0) continue;

                if (validate is null || validate(input)) return input;

                error = errorMessage ?? "Invalid input.";
                continue;
            }

            if (key.Key == ConsoleKey.Backspace)
            {
                if (input.Length > 0) input = input[..^1];
                error = null;
                continue;
            }

            if (!char.IsControl(key.KeyChar))
            {
                input += key.KeyChar;
                error = null;
            }
        }
    }
}