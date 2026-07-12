using PRA.CLI.Components;
using PRA.CLI.Input;
using PRA.CLI.Services;
using PRA.CLI.Viewers;
using Spectre.Console;

namespace PRA.CLI.Screens;

public class MainMenu {

    public void Show() {
        Header.Draw();

        List<int> reference = UserInput.ReadReferenceString();
        int frames = UserInput.ReadFrameCount();

        var algorithm =
            AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Choose Algorithm[/]")
                    .PageSize(10)
                    .AddChoices(
                        AlgorithmFactory
                            .GetAlgorithms()
                            .Select(a => a.Name)));

        var selected =
            AlgorithmFactory
                .GetAlgorithms()
                .First(a => a.Name == algorithm);

        var runner = new SimulationRunner();
        
        var result = runner.Run(selected, reference, frames);

        new SimulationViewer().Show(
            result,
            reference);
    }

}