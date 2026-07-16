using PRA.CLI.Components;
using PRA.CLI.Screens;
using PRA.CLI.Services;

Console.Title = "Page Replacement Algorithm Simulator";

// Program.cs
string? saved = SettingsService.LoadPrimaryColor();
if (saved is not null) Theme.SetPrimary(saved);

new TitleScreen().Show();
new MainMenu().Show();