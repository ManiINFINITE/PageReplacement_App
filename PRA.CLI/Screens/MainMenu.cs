using PRA.CLI.Components;
using PRA.CLI.Input;
using PRA.CLI.Services;
using PRA.CLI.Viewers;
using Spectre.Console;

namespace PRA.CLI.Screens;

public class MainMenu {

    public void Show() {
        while (true) {
            AnsiConsole.Clear();
            Header.Draw();

            List<int> reference = UserInput.ReadReferenceString();
            int frames = UserInput.ReadFrameCount();

            string mode = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]What do you want to do?[/]")
                    .AddChoices(
                        "Run a single algorithm",
                        "Compare all algorithms",
                        "Compare two algorithms side by side"
                    ));

            var algorithms = AlgorithmFactory.GetAlgorithms();
            var runner = new SimulationRunner();

            switch (mode) {
                case "Compare all algorithms":
                    new CompareViewer().Show(reference, frames);
                    break;

                case "Compare two algorithms side by side": {
                    var firstName = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("[yellow]Choose first algorithm[/]")
                            .AddChoices(algorithms.Select(a => a.Name)));

                    var secondName = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("[yellow]Choose second algorithm[/]")
                            .AddChoices(algorithms.Select(a => a.Name).Where(n => n != firstName)));

                    var algoA = algorithms.First(a => a.Name == firstName);
                    var algoB = algorithms.First(a => a.Name == secondName);

                    var resultA = runner.Run(algoA, reference, frames);
                    var resultB = runner.Run(algoB, reference, frames);

                    new SideBySideViewer().Show(resultA, resultB, reference);
                    break;
                }

                default: {
                    var algorithmName = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("[yellow]Choose Algorithm[/]")
                            .PageSize(10)
                            .AddChoices(algorithms.Select(a => a.Name)));

                    var selected = algorithms.First(a => a.Name == algorithmName);
                    var result = runner.Run(selected, reference, frames);

                    new SimulationViewer().Show(result, reference);
                    break;
                }
            }
        }
    }

}