using Spectre.Console;

namespace PRA.CLI.Components;

public static class ReferencePanel {

    public static Panel Build(
        IReadOnlyList<int> referenceString,
        int currentStep
    ) {
        var builder = new System.Text.StringBuilder();

        for (int i = 0; i < referenceString.Count; i++) {
            if (i == currentStep) {
                builder.Append($"[black on yellow]{referenceString[i]}[/] ");
            } else {
                builder.Append($"{referenceString[i]} ");
            }
        }

        return new Panel(new Markup(builder.ToString())) {
            Header = new PanelHeader("Reference String"),
            Border = BoxBorder.Rounded
        };
    }

}