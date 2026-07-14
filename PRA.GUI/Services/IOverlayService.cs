using PRA.GUI.ViewModels;

namespace PRA.GUI.Services;

/// <summary>
/// Lets any page view-model show a modal overlay on top of the whole shell
/// (the same scrim + centered card used by "New Simulation"), without needing
/// a reference to MainWindowViewModel itself.
/// </summary>
public interface IOverlayService {

    ViewModelBase? CurrentViewModel { get; }
    bool IsVisible { get; }

    void Open(ViewModelBase viewModel);
    void Close();

}
