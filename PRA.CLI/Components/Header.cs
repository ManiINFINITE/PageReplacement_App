using Spectre.Console;

namespace PRA.CLI.Components;

public static class Header {

    public static void Draw() {
        AnsiConsole.Write(
            new Rule("[bold cornflowerblue]PAGE REPLACEMENT SIMULATOR[/]")
                .Centered()
                .RuleStyle(Theme.Accent));
    }

}