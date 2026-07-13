using Spectre.Console;

namespace PRA.CLI.Components;

public static class Theme {

    // Change this one line to re-theme the entire app.
    public const string PrimaryName = "DodgerBlue1";

    public static Color ColorFromName(string name) {
        return Style.Parse(name).Foreground;
    }

    public readonly static Color HitColor = ColorFromName(HitName);
    public readonly static Color FaultColor = ColorFromName(FaultName);
    public readonly static Color WarnColor = ColorFromName(WarnName);
    public readonly static Color PrimaryColor = ColorFromName(PrimaryName);

    public const string DimName = "grey";
    public const string HitName = "green3";
    public const string FaultName = "red3";
    public const string WarnName = "gold1";

    public readonly static Style BorderStyle = Style.Parse(PrimaryName);

    public const string HitTag = HitName;
    public const string FaultTag = FaultName;
    public const string CurrentTag = "black on " + PrimaryName;
    public const string ReplacedTag = "black on " + WarnName;
    public const string DimTag = DimName;

    public static string Bold(string text) {
        return $"[bold {PrimaryName}]{text}[/]";
    }

    public static string Accent(string text) {
        return $"[{PrimaryName}]{text}[/]";
    }

    public static string Dim(string text) {
        return $"[{DimName}]{text}[/]";
    }

    public static string Fault(string text) {
        return $"[{FaultName}]{text}[/]";
    }

    public static string Hit(string text) {
        return $"[{HitName}]{text}[/]";
    }

    public static string Warn(string text) {
        return $"[{WarnName}]{text}[/]";
    }

    public static BoxBorder Border => BoxBorder.Double;

}