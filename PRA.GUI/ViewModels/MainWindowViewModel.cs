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
    private IPageReplacementAlgorithm _algorithm = new FifoAlgorithm();

    public INavigationService Navigation { get; }
    public IOverlayService Overlay { get; }

    [ObservableProperty] private DashboardViewModel dashboard;
    [ObservableProperty] private bool isSidebarCollapsed;
    [ObservableProperty] private string _collapseText;
    [ObservableProperty] private string _themeIcon;

    public IRelayCommand OpenNewSimulationCommand { get; }
    public IRelayCommand ToggleSidebarCommand { get; }

    private string _currentTheme;

    public MainWindowViewModel()
    {
        Overlay = new OverlayService();

        // Placeholder input until the GUI has its own setup screen
        var reference = new List<int> { 7, 0, 1, 2, 0, 3, 0, 4, 2, 3, 0, 3, 2 };
        var result = _algorithm.Run(reference, 3);

        // Pass Overlay to DashboardViewModel
        dashboard = new DashboardViewModel(result, reference, Overlay);

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
        // Pass Overlay to DashboardViewModel
        Dashboard = new DashboardViewModel(result, referenceString, Overlay);

        Navigation.Register(AppPage.Dashboard, () => Dashboard, false);
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