using Spectre.Console;

namespace PRA.CLI.Components;

public static class Theme
{
    public static string PrimaryName { get; private set; } = "green3";

    public const string DimName = "grey";
    public const string FaultName = "red3";
    public const string WarnName = "gold1";

    public readonly static (string Name, string Swatch)[] AvailableColors =
    [
        ("green3", "green3"),
        ("DodgerBlue1", "DodgerBlue1"),
        ("orange1", "orange1"),
        ("mediumpurple2", "mediumpurple2"),
        ("gold1", "gold1"),
        ("red3", "red3"),
        ("aqua", "aqua"),
        ("hotpink", "hotpink"),
        ("lightgoldenrod1", "lightgoldenrod1"),
        ("white", "white")
    ];

    public static void SetPrimary(string colorName)
    {
        PrimaryName = colorName;
    }

    public static Style BorderStyle => Style.Parse(PrimaryName);
    public static Color PrimaryColor => Style.Parse(PrimaryName).Foreground;
    public static Color FaultColor => Style.Parse(FaultName).Foreground;
    public static Color WarnColor => Style.Parse(WarnName).Foreground;
    public static Color HitColor => PrimaryColor;

    public static string HitTag => PrimaryName;
    public static string FaultTag => FaultName;
    public static string CurrentTag => $"black on {PrimaryName}";
    public static string ReplacedTag => $"black on {WarnName}";
    public static string DimTag => DimName;

    public static string Bold(string text)
    {
        return $"[bold {PrimaryName}]{text}[/]";
    }

    public static string Accent(string text)
    {
        return $"[{PrimaryName}]{text}[/]";
    }

    public static string Dim(string text)
    {
        return $"[{DimName}]{text}[/]";
    }

    public static string Fault(string text)
    {
        return $"[{FaultName}]{text}[/]";
    }

    public static string Hit(string text)
    {
        return $"[{PrimaryName}]{text}[/]";
    }

    public static string Warn(string text)
    {
        return $"[{WarnName}]{text}[/]";
    }

    public static BoxBorder Border => BoxBorder.Double;
}