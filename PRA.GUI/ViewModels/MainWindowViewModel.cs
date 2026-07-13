using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PRA.Core.Algorithms;
using PRA.Core.Interfaces;
using PRA.GUI.Services;
using System.Collections.Generic;

namespace PRA.GUI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase {

    // Same algorithm that's currently on the Dashboard — reused whenever a
    // new simulation is started from the overlay (no algorithm picker yet).
    private IPageReplacementAlgorithm _algorithm = new FifoAlgorithm();

    public INavigationService Navigation { get; }

    // Kept as a standalone property (rather than only living inside the
    // navigation cache) because the sidebar's "ALGORITHM" card shows it
    // regardless of which page is currently active.
    [ObservableProperty] private DashboardViewModel dashboard;

    // Null when no overlay is open. Bind a ContentControl to this in the shell.
    [ObservableProperty] private ViewModelBase? overlayViewModel;

    public bool IsOverlayVisible => OverlayViewModel is not null;

    public IRelayCommand OpenNewSimulationCommand { get; }

    public MainWindowViewModel() {
        // Placeholder input until the GUI has its own setup screen — swap for
        // whatever the user picks once you build that flow.
        var reference = new List<int> { 7, 0, 1, 2, 0, 3, 0, 4, 2, 3, 0, 3, 2 };
        var result = _algorithm.Run(reference, 3);

        dashboard = new DashboardViewModel(result, reference);

        Navigation = new NavigationService();
        Navigation.Register(AppPage.Dashboard, () => Dashboard);
        Navigation.Register(AppPage.CompareAll, () => new CompareAllViewModel());
        Navigation.Register(AppPage.CompareTwo, () => new CompareTwoViewModel());
        Navigation.Register(AppPage.Settings, () => new SettingsViewModel());

        Navigation.NavigateTo(AppPage.Dashboard);

        OpenNewSimulationCommand = new RelayCommand(OpenNewSimulation);
    }

    partial void OnOverlayViewModelChanged(ViewModelBase? value) => OnPropertyChanged(nameof(IsOverlayVisible));

    private void OpenNewSimulation() {
        OverlayViewModel = new NewSimulationViewModel(_algorithm, StartNewSimulation, CloseOverlay);
    }

    private void CloseOverlay() {
        OverlayViewModel = null;
    }

    private void StartNewSimulation(List<int> referenceString, int frameCount, IPageReplacementAlgorithm algorithm) {
        _algorithm = algorithm;
        var result = _algorithm.Run(referenceString, frameCount);
        Dashboard = new DashboardViewModel(result, referenceString);

        Navigation.Register(AppPage.Dashboard, () => Dashboard, cache: false);
        Navigation.NavigateTo(AppPage.Dashboard);

        CloseOverlay();
    }

}
