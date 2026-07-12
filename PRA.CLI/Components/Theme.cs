using Spectre.Console;

namespace PRA.CLI.Components;

public static class Theme {

    public readonly static Style Title = new(Color.CornflowerBlue);
    public readonly static Style Success = new(Color.Green);
    public readonly static Style Error = new(Color.Red);
    public readonly static Style Warning = new(Color.Yellow);
    public readonly static Style Accent = new(Color.DeepSkyBlue1);

}