using PRA.CLI.Components;
using PRA.CLI.Input;
using PRA.CLI.Screens;
using PRA.CLI.Services;
using PRA.Core.Models;
using Spectre.Console;

namespace PRA.CLI.Viewers;

public class CompareViewer {

    readonly private static Color[] Palette = [
        Color.Green3, Color.DeepSkyBlue1, Color.Orange1,
        Color.MediumPurple2, Color.Gold1, Color.Red3, Color.Aqua
    ];

    public void Show(IReadOnlyList<int> referenceString, int frameCount) {
        var runner = new SimulationRunner();
        var algorithms = AlgorithmFactory.GetAlgorithms();

        var colorByAlgorithm = algorithms
            .Select((a, i) => (a.Name, Color: Palette[i % Palette.Length]))
            .ToDictionary(x => x.Name, x => x.Color);

        var results = algorithms
            .Select(a => runner.Run(a, referenceString, frameCount))
            .OrderByDescending(r => r.HitRatio)
            .ToList();

        while (true) {
            AnsiConsole.Clear();
            Header.Draw();

            AnsiConsole.Write(Align.Center(new Markup(
                $"{Theme.Dim("Reference:")} {string.Join(' ', referenceString)}   " +
                $"{Theme.Dim("Frames:")} {frameCount}")));

            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine();

            var table = new Table()
                .Border(TableBorder.Double)
                .BorderStyle(Theme.BorderStyle)
                .Expand()
                .Title(Theme.Bold("RESULTS"));

            table.AddColumn(new TableColumn(Theme.Bold("Algorithm")));
            table.AddColumn(new TableColumn(Theme.Bold("Hits")).Centered());
            table.AddColumn(new TableColumn(Theme.Bold("Faults")).Centered());
            table.AddColumn(new TableColumn(Theme.Bold("Hit Ratio")).Centered());

            foreach (var r in results) {
                var swatch = colorByAlgorithm[r.AlgorithmName];

                table.AddRow(
                    new Markup($"[{swatch.ToMarkup()}]■[/] {r.AlgorithmName}"),
                    Align.Center(new Markup(Theme.Hit(r.PageHits.ToString()))),
                    Align.Center(new Markup(Theme.Fault(r.PageFaults.ToString()))),
                    Align.Center(new Markup($"{r.HitRatio:P0}"))
                );
            }

            AnsiConsole.Write(Align.Center(table));
            AnsiConsole.WriteLine();

            var chart = new BreakdownChart()
                .Width(60)
                .FullSize()
                .UseValueFormatter(v => $"{v:0}%");

            foreach (var r in results)
                chart.AddItem(r.AlgorithmName, Math.Round(r.HitRatio * 100, 1), colorByAlgorithm[r.AlgorithmName]);

            AnsiConsole.Write(Align.Center(chart));
            AnsiConsole.WriteLine();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title(Theme.Warn("What next?"))
                    .AddChoices(
                        results.Select(r => r.AlgorithmName)
                            .Append("Export comparison")
                            .Append("[grey]Back[/]")));

            if (choice == "[grey]Back[/]") return;

            if (choice == "Export comparison") {
                string csv = ExportService.ComparisonToCsv(results, referenceString);
                string md = ExportService.ComparisonToMarkdown(results, referenceString);

                string? path = ExportPrompt.Run("comparison", csv, md);

                AnsiConsole.MarkupLine(path is not null
                    ? $"{Theme.Hit("Saved to")} {path}"
                    : Theme.Dim("Export cancelled."));

                AnsiConsole.MarkupLine(Theme.Dim("Press any key to continue..."));
                Console.ReadKey(true);
                continue;
            }

            var selected = results.First(r => r.AlgorithmName == choice);
            new SimulationViewer().Show(selected, referenceString);
        }
    }

}