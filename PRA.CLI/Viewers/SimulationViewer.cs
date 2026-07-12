using PRA.CLI.Components;
using PRA.Core.Models;
using Spectre.Console;

namespace PRA.CLI.Viewers;

public class SimulationViewer {

    public void Show(
        SimulationResult result,
        IReadOnlyList<int> referenceString
    ) {
        int currentStep = 0;

        while (true) {
            AnsiConsole.Clear();

            Header.Draw();

            Draw(result, referenceString, currentStep);

            Footer.Draw();

            var key = Console.ReadKey(true);

            switch (key.Key) {
                case ConsoleKey.RightArrow:
                    if (currentStep < result.Steps.Count - 1) currentStep++;
                    break;
                case ConsoleKey.LeftArrow:
                    if (currentStep > 0) currentStep--;
                    break;
                case ConsoleKey.Home:
                    currentStep = 0;
                    break;
                case ConsoleKey.End:
                    currentStep = result.Steps.Count - 1;
                    break;
                case ConsoleKey.Escape:
                    return;
            }
        }
    }

    private static void Draw(
        SimulationResult result,
        IReadOnlyList<int> referenceString,
        int currentStep
    ) {
        SimulationStep step = result.Steps[currentStep];

        var info = new Grid();

        info.AddColumn();
        info.AddColumn();

        info.AddRow(
            "[cyan]Algorithm[/]",
            result.AlgorithmName);

        info.AddRow(
            "[cyan]Step[/]",
            $"{currentStep + 1}/{result.Steps.Count}");

        info.AddRow(
            "[cyan]Current Page[/]",
            step.CurrentPage.ToString());

        info.AddRow(
            "[cyan]Result[/]",
            step.IsPageFault
                ? "[red]Page Fault[/]"
                : "[green]Page Hit[/]");

        info.AddRow(
            "[cyan]Replaced[/]",
            step.ReplacedPage?.ToString() ?? "-");

        var left = new Rows(
            new Panel(info) {
                Header = new PanelHeader("Information"),
                Border = BoxBorder.Rounded
            },
            ReferencePanel.Build(
                referenceString,
                currentStep)
        );

        var right = new Rows(
            FramePanel.Build(step),
            StatisticsPanel.Build(
                result,
                currentStep)
        );

        var layout = new Layout();

        layout.SplitColumns(
            new Layout("Left")
                .Ratio(2),
            new Layout("Right")
                .Ratio(1)
        );

        layout["Left"].Update(left);

        layout["Right"].Update(right);

        AnsiConsole.Write(layout);
    }

}