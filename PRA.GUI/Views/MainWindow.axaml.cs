using Avalonia.Controls;
using Avalonia.Input;
using PRA.GUI.ViewModels;

namespace PRA.GUI.Views;

public partial class MainWindow : Window {

    public MainWindow() {
        InitializeComponent();
        KeyDown += OnKeyDown;
    }

    private void OnKeyDown(object? sender, KeyEventArgs e) {
        if (DataContext is not MainWindowViewModel vm) return;

        switch (e.Key) {
            case Key.Right: vm.Dashboard.NextStepCommand.Execute(null); break;
            case Key.Left:  vm.Dashboard.PreviousStepCommand.Execute(null); break;
            case Key.Home:  vm.Dashboard.JumpToStartCommand.Execute(null); break;
            case Key.End:   vm.Dashboard.JumpToEndCommand.Execute(null); break;
        }
    }

}