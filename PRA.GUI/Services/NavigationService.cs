using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PRA.GUI.ViewModels;

namespace PRA.GUI.Services;

public partial class NavigationService : ObservableObject, INavigationService {

    private readonly Dictionary<AppPage, Func<ViewModelBase>> _factories = new();
    private readonly Dictionary<AppPage, ViewModelBase> _cache = new();

    [ObservableProperty] private ViewModelBase? currentViewModel;
    [ObservableProperty] private AppPage currentPage;

    public IRelayCommand<AppPage> NavigateCommand { get; }

    public NavigationService() {
        NavigateCommand = new RelayCommand<AppPage>(NavigateTo);
    }

    public void Register(AppPage page, Func<ViewModelBase> factory, bool cache = true) {
        _factories[page] = factory;

        if (!cache) {
            _cache.Remove(page);
        }
    }

    public void NavigateTo(AppPage page) {
        if (!_factories.TryGetValue(page, out var factory)) {
            return;
        }

        if (!_cache.TryGetValue(page, out var viewModel)) {
            viewModel = factory();
            _cache[page] = viewModel;
        }

        CurrentViewModel = viewModel;
        CurrentPage = page;
    }

}
