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
                    .AddChoices("Run a single algorithm", "Compare all algorithms"));

            if (mode == "Compare all algorithms") {
                new CompareViewer().Show(reference, frames);
            } else {
                var algorithms = AlgorithmFactory.GetAlgorithms();

                var algorithmName = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Choose Algorithm[/]")
                        .PageSize(10)
                        .AddChoices(algorithms.Select(a => a.Name)));

                var selected = algorithms.First(a => a.Name == algorithmName);
                var result = new SimulationRunner().Run(selected, reference, frames);

                new SimulationViewer().Show(result, reference);
            }

            bool again = AnsiConsole.Confirm("[cyan]Run another simulation?[/]", true);
            if (!again) return;
        }
    }

}