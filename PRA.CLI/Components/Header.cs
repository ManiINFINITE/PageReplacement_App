using Spectre.Console;

namespace PRA.CLI.Components;

public static class Header {

    public static void Draw() {
        AnsiConsole.Write(
            new Rule(Theme.Bold("PAGE REPLACEMENT ALGORITHM SIMULATOR"))
                .Centered()
                .RuleStyle(Theme.BorderStyle));
    }

}