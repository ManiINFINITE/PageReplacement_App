using CommunityToolkit.Mvvm.ComponentModel;
using PRA.GUI.ViewModels;

namespace PRA.GUI.Services;

public partial class OverlayService : ObservableObject, IOverlayService {

    [ObservableProperty] private ViewModelBase? currentViewModel;

    public bool IsVisible => CurrentViewModel is not null;

    partial void OnCurrentViewModelChanged(ViewModelBase? value) => OnPropertyChanged(nameof(IsVisible));

    public void Open(ViewModelBase viewModel) {
        CurrentViewModel = viewModel;
    }

    public void Close() {
        CurrentViewModel = null;
    }

}
