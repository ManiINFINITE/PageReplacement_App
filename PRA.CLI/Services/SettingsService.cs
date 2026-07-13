// Services/SettingsService.cs

using System.Text.Json;

namespace PRA.CLI.Services;

public static class SettingsService {

    readonly private static string Path =
        System.IO.Path.Combine(AppContext.BaseDirectory, "settings.json");

    public static void SavePrimaryColor(string colorName) =>
        File.WriteAllText(Path, JsonSerializer.Serialize(new { PrimaryColor = colorName }));

    public static string? LoadPrimaryColor() {
        if (!File.Exists(Path)) return null;

        var doc = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(Path));
        return doc?.GetValueOrDefault("PrimaryColor");
    }

}