using PRA.Core.Utilities;
using Spectre.Console;

namespace PRA.CLI.Input;

public static class UserInput {

    public static List<int> ReadReferenceString() {
        while (true) {
            string input = AnsiConsole.Ask<string>("[cyan]Reference String[/]");

            try {
                return ListConverter.ConvertToIntArray(input);
            } catch {
                AnsiConsole.MarkupLine("[red]Invalid reference string.[/]");
            }
        }
    }

    public static int ReadFrameCount() {
        while (true) {
            int value = AnsiConsole.Ask<int>("[cyan]Frame Count[/]");

            if (value > 0) return value;

            AnsiConsole.MarkupLine("[red]Frame count must be greater than zero.[/]");
        }
    }

}