using PRA.CLI.Components;
using PRA.CLI.Input;
using PRA.CLI.Services;
using PRA.CLI.Viewers;

namespace PRA.CLI.Screens;

public class MainMenu
{
    public void Show()
    {
        while (true)
        {
            string choice = BorderedMenu.Show("Main Menu",
            [
                "New single simulation",
                "Compare all algorithms",
                "Compare two algorithms",
                "Settings",
                "Close app"
            ]);

            if (choice == "Close app") return;

            if (choice == "Settings")
            {
                new SettingsScreen().Show();
                continue;
            }

            var reference = UserInput.ReadReferenceString();
            int frames = UserInput.ReadFrameCount();

            var algorithms = AlgorithmFactory.GetAlgorithms();
            var runner = new SimulationRunner();

            switch (choice)
            {
                case "Compare all algorithms":
                    new CompareViewer().Show(reference, frames);
                    break;

                case "Compare two algorithms":
                {
                    string firstName = BorderedMenu.Show(
                        "Choose first algorithm",
                        algorithms.Select(a => a.Name).ToList());

                    string secondName = BorderedMenu.Show(
                        "Choose second algorithm",
                        algorithms.Select(a => a.Name).Where(n => n != firstName).ToList());

                    var algoA = algorithms.First(a => a.Name == firstName);
                    var algoB = algorithms.First(a => a.Name == secondName);

                    var resultA = runner.Run(algoA, reference, frames);
                    var resultB = runner.Run(algoB, reference, frames);

                    new SideBySideViewer().Show(resultA, resultB, reference);
                    break;
                }

                default:
                {
                    string algorithmName = BorderedMenu.Show(
                        "Choose algorithm",
                        algorithms.Select(a => a.Name).ToList());

                    var selected = algorithms.First(a => a.Name == algorithmName);
                    var result = runner.Run(selected, reference, frames);

                    new SimulationViewer().Show(result, reference);
                    break;
                }
            }
        }
    }
}