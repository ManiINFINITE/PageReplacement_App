using PRA.Core.Models;
using Spectre.Console;

namespace PRA.CLI.Components;

public static class ReferencePanel {

    public static Panel Build(
        IReadOnlyList<int> referenceString,
        IReadOnlyList<SimulationStep> steps,
        int currentStep
    ) {
        var sb = new System.Text.StringBuilder();

        for (int i = 0; i < referenceString.Count; i++) {
            string page = referenceString[i].ToString();

            if (i == currentStep) {
                sb.Append($"[{Theme.CurrentTag}] {page} [/]");
            } else if (i < currentStep) {
                bool fault = steps[i].IsPageFault;
                sb.Append($"[{(fault ? Theme.FaultTag : Theme.HitTag)}]{page}[/]");
            } else {
                sb.Append($"[{Theme.DimTag}]{page}[/]");
            }

            sb.Append(' ');
        }

        // ReferencePanel.cs
        return new Panel(Align.Center(new Markup(sb.ToString()))) {
            Header = new PanelHeader("REFERENCE STRING", Justify.Center),
            Border = BoxBorder.Rounded,
            Expand = true,
            Padding = new Padding(2, 0, 2, 1)
        };
    }

}