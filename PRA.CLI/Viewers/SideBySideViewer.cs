using PRA.CLI.Components;
using PRA.Core.Models;
using Spectre.Console;

namespace PRA.CLI.Viewers;

public class SideBySideViewer
{
    public void Show(
        SimulationResult resultA,
        SimulationResult resultB,
        IReadOnlyList<int> referenceString
    )
    {
        int currentStep = 0;
        int maxStep = resultA.Steps.Count - 1;

        AnsiConsole.Clear();

        while (true)
        {
            AnsiConsole.Cursor.SetPosition(0, 0);
            Draw(resultA, resultB, referenceString, currentStep);

            var key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.RightArrow:
                    if (currentStep < maxStep) currentStep++;
                    break;
                case ConsoleKey.LeftArrow:
                    if (currentStep > 0) currentStep--;
                    break;
                case ConsoleKey.Home: currentStep = 0; break;
                case ConsoleKey.End: currentStep = maxStep; break;
                case ConsoleKey.Escape: return;
            }
        }
    }

    private static void Draw(
        SimulationResult resultA,
        SimulationResult resultB,
        IReadOnlyList<int> referenceString,
        int currentStep
    )
    {
        int frameCount = resultA.Steps[currentStep].Frames.Count;
        int bodyHeight = 4 + frameCount + 6 + 9;

        var root = new Layout("Root")
            .SplitRows(
                new Layout("Header").Size(12),
                new Layout("Body").Size(bodyHeight),
                new Layout("Footer").Size(3)
            );

        root["Header"].SplitColumns(
            new Layout("HeaderA"),
            new Layout("HeaderB")
        );

        root["Header"]["HeaderA"].Update(InfoPanel.Build(resultA, currentStep));
        root["Header"]["HeaderB"].Update(InfoPanel.Build(resultB, currentStep));

        root["Body"].SplitColumns(
            new Layout("ColA").SplitRows(
                new Layout("RefA").Size(4),
                new Layout("FramesA").Ratio(1),
                new Layout("StatsA").Size(9)
            ),
            new Layout("ColB").SplitRows(
                new Layout("RefB").Size(4),
                new Layout("FramesB").Ratio(1),
                new Layout("StatsB").Size(9)
            )
        );

        root["Body"]["ColA"]["RefA"].Update(ReferencePanel.Build(referenceString, resultA.Steps, currentStep));
        root["Body"]["ColA"]["FramesA"].Update(FramePanel.Build(resultA.Steps[currentStep]));
        root["Body"]["ColA"]["StatsA"].Update(StatisticsPanel.Build(resultA, currentStep));

        root["Body"]["ColB"]["RefB"].Update(ReferencePanel.Build(referenceString, resultB.Steps, currentStep));
        root["Body"]["ColB"]["FramesB"].Update(FramePanel.Build(resultB.Steps[currentStep]));
        root["Body"]["ColB"]["StatsB"].Update(StatisticsPanel.Build(resultB, currentStep));

        root["Footer"].Update(Footer.Build());

        AnsiConsole.Write(root);
    }
}