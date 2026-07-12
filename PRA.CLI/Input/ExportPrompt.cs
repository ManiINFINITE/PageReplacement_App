using Spectre.Console;
using PRA.CLI.Services;

namespace PRA.CLI.Input;

public static class ExportPrompt {

    public static string? Run(string defaultFileNameBase, string csvContent, string markdownContent) {
        var format = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Export format[/]")
                .AddChoices("CSV", "Markdown", "[grey]Cancel[/]"));

        if (format == "[grey]Cancel[/]") return null;

        string extension = format == "CSV" ? "csv" : "md";
        string content = format == "CSV" ? csvContent : markdownContent;

        string fileName = AnsiConsole.Ask(
            "[cyan]File name[/]",
            $"{defaultFileNameBase}.{extension}");

        if (!fileName.EndsWith($".{extension}")) fileName += $".{extension}";

        return ExportService.Save(content, fileName);
    }

}