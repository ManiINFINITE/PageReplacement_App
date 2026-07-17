using Avalonia;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PRA.Core.Algorithms;
using PRA.Core.Interfaces;
using PRA.GUI.Services;
using System.Collections.Generic;

namespace PRA.GUI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    // Whatever's currently on the Dashboard — preselected in the New
    // Simulation overlay and reassignable once the user picks a different
    // algorithm there.
    private IPageReplacementAlgorithm _algorithm = new FifoAlgorithm();

    public INavigationService Navigation { get; }

    // Shared with page view-models (Compare All, etc.) so they can pop their
    // own overlays without needing a reference back to the shell.
    public IOverlayService Overlay { get; }

    // Kept as a standalone property (rather than only living inside the
    // navigation cache) because the sidebar's "ALGORITHM" card shows it
    // regardless of which page is currently active.
    [ObservableProperty] private DashboardViewModel dashboard;
    [ObservableProperty] private bool isSidebarCollapsed;
    [ObservableProperty] private string _collapseText;
    [ObservableProperty] private string _themeIcon;

    public IRelayCommand OpenNewSimulationCommand { get; }
    public IRelayCommand ToggleSidebarCommand { get; }

    private string _currentTheme;

    public MainWindowViewModel()
    {
        // Placeholder input until the GUI has its own setup screen — swap for
        // whatever the user picks once you build that flow.
        var reference = new List<int> { 7, 0, 1, 2, 0, 3, 0, 4, 2, 3, 0, 3, 2 };
        var result = _algorithm.Run(reference, 3);

        dashboard = new DashboardViewModel(result, reference);

        Overlay = new OverlayService();

        Navigation = new NavigationService();
        Navigation.Register(AppPage.Dashboard, () => Dashboard);
        Navigation.Register(AppPage.CompareAll, () => new CompareAllViewModel(Overlay));
        Navigation.Register(AppPage.CompareTwo, () => new CompareTwoViewModel(Overlay));

        Navigation.NavigateTo(AppPage.Dashboard);

        OpenNewSimulationCommand = new RelayCommand(OpenNewSimulation);

        ToggleSidebarCommand = new RelayCommand(() =>
        {
            IsSidebarCollapsed = !IsSidebarCollapsed;
            CollapseText = IsSidebarCollapsed ? "Expand" : "Collapse";
        });

        _currentTheme = Application.Current!.RequestedThemeVariant == ThemeVariant.Dark ? "Dark" : "Light";
        _collapseText = IsSidebarCollapsed ? "Expand" : "Collapse";
        _themeIcon = _currentTheme == "Dark" ? "\uE330" : "\uE474";
    }

    private void OpenNewSimulation()
    {
        Overlay.Open(new NewSimulationViewModel(_algorithm, StartNewSimulation, Overlay.Close));
    }

    private void StartNewSimulation(List<int> referenceString, int frameCount, IPageReplacementAlgorithm algorithm)
    {
        _algorithm = algorithm;

        var result = _algorithm.Run(referenceString, frameCount);
        Dashboard = new DashboardViewModel(result, referenceString);

        // Re-point the Dashboard page at the fresh view-model and jump there.
        Navigation.Register(AppPage.Dashboard, () => Dashboard, cache: false);
        Navigation.NavigateTo(AppPage.Dashboard);

        Overlay.Close();
    }

    [RelayCommand]
    private void ToggleTheme()
    {
        switch (_currentTheme)
        {
            case "Dark":
                Application.Current!.RequestedThemeVariant = ThemeVariant.Light;
                _currentTheme = "Light";
                ThemeIcon = "\uE474";
                break;
            case "Light":
                Application.Current!.RequestedThemeVariant = ThemeVariant.Dark;
                _currentTheme = "Dark";
                ThemeIcon = "\uE330";
                break;
        }
    }
}