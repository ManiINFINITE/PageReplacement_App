using Avalonia.Controls;
using Avalonia.Input;
using PRA.GUI.ViewModels;

namespace PRA.GUI.Views;

public partial class MainWindowView : Window {

    public MainWindowView() {
        InitializeComponent();
        KeyDown += OnKeyDown;
    }

    private void OnKeyDown(object? sender, KeyEventArgs e) {
        if (DataContext is not MainWindowViewModel vm) return;

        // Playback shortcuts only make sense while the Dashboard page is active.
        if (vm.Navigation.CurrentViewModel is not DashboardViewModel dashboard) return;

        switch (e.Key) {
            case Key.Right: dashboard.NextStepCommand.Execute(null); break;
            case Key.Left:  dashboard.PreviousStepCommand.Execute(null); break;
            case Key.Home:  dashboard.JumpToStartCommand.Execute(null); break;
            case Key.End:   dashboard.JumpToEndCommand.Execute(null); break;
        }
    }

}
