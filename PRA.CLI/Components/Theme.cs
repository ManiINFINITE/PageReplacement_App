using Spectre.Console;

namespace PRA.CLI.Components;

public static class Theme {

    public readonly static Style Title = new(Color.CornflowerBlue, decoration: Decoration.Bold);
    public readonly static Style Accent = new(Color.DeepSkyBlue1);
    public readonly static Style Muted = new(Color.Grey);
    public readonly static Style Dim = new(Color.Grey50);

    public readonly static Style Hit = new(Color.Green);
    public readonly static Style Fault = new(Color.Red);
    public readonly static Style Current = new(Color.Black, Color.Yellow);
    public readonly static Style Replaced = new(Color.Black, Color.Orange1);

    public const string HitTag = "green";
    public const string FaultTag = "red";
    public const string CurrentTag = "black on yellow";
    public const string ReplacedTag = "black on orange1";
    public const string DimTag = "grey50";

}