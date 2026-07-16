using PRA.CLI.Components;
using PRA.CLI.Input;
using PRA.CLI.Services;
using PRA.Core.Models;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace PRA.CLI.Viewers;

public class SimulationViewer
{
    private bool _showOverview = false;

    public void Show(SimulationResult result, IReadOnlyList<int> referenceString)
    {
        int currentStep = 0;
        AnsiConsole.Clear();

        while (true)
        {
            AnsiConsole.Cursor.SetPosition(0, 0); // avoid full clear-flicker every frame
            Draw(result, referenceString, currentStep);

            var key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.RightArrow:
                    if (currentStep < result.Steps.Count - 1) currentStep++;
                    break;
                case ConsoleKey.LeftArrow:
                    if (currentStep > 0) currentStep--;
                    break;
                case ConsoleKey.E:
                {
                    string csv = ExportService.ToCsv(result, referenceString);
                    string md = ExportService.ToMarkdown(result, referenceString);
                    string safeName = result.AlgorithmName.Replace(" ", "_").Replace("(", "").Replace(")", "");

                    string? path = ExportPrompt.Run(safeName, csv, md);

                    AnsiConsole.Clear();

                    if (path is not null)
                        AnsiConsole.MarkupLine($"[green]Saved to[/] {path}");
                    else
                        AnsiConsole.MarkupLine("[grey]Export cancelled.[/]");

                    AnsiConsole.MarkupLine("[grey]Press aby key to continue...[/]");
                    Console.ReadKey(true);
                    break;
                }
                case ConsoleKey.Home: currentStep = 0; break;
                case ConsoleKey.End: currentStep = result.Steps.Count - 1; break;
                case ConsoleKey.T: _showOverview = !_showOverview; break;
                case ConsoleKey.Escape: return;
            }
        }
    }

    // SimulationViewer.cs
    private void Draw(SimulationResult result, IReadOnlyList<int> referenceString, int currentStep)
    {
        int frameCount = result.Steps[currentStep].Frames.Count;

        // Size the body to what the content actually needs, not the terminal height.
        int historyRows = frameCount + 6; // border + header row + one row per frame + border
        int framesRows = frameCount + 8; // reference panel + frame table
        int statsRows = 10; // grid + breakdown chart
        int bodyHeight = new[] { historyRows, framesRows, statsRows }.Max();

        var root = new Layout("Root")
            .SplitRows(
                new Layout("Header").Size(12),
                new Layout("Body").Size(bodyHeight),
                new Layout("Footer").Size(3)
            );

        root["Header"].Update(InfoPanel.Build(result, currentStep));

        root["Body"].SplitColumns(
            new Layout("Left").Ratio(3)
                .SplitRows(
                    new Layout("Reference").Size(4),
                    new Layout("Frames").Ratio(1)
                ),
            new Layout("Middle").Ratio(4),
            new Layout("Right").Ratio(3)
        );

        root["Body"]["Left"]["Reference"].Update(
            ReferencePanel.Build(referenceString, result.Steps, currentStep));

        root["Body"]["Left"]["Frames"].Update(
            FramePanel.Build(result.Steps[currentStep]));

        root["Body"]["Middle"].Update(
            HistoryTable.Build(result, referenceString, currentStep));

        root["Body"]["Right"].Update(
            StatisticsPanel.Build(result, currentStep));

        root["Footer"].Update(Footer.Build());

        AnsiConsole.Write(root);
    }
}