using System;
using CommunityToolkit.Mvvm.Input;
using PRA.GUI.ViewModels;

namespace PRA.GUI.Services;

/// <summary>
/// Drives which page view-model is currently on screen. The navbar (and anything
/// else) calls <see cref="NavigateTo"/> or executes <see cref="NavigateCommand"/>
/// to switch pages; the shell just renders <see cref="CurrentViewModel"/> via a
/// ContentControl + ViewLocator.
/// </summary>
public interface INavigationService
{
    /// <summary>The view-model currently on screen. Bind a ContentControl to this.</summary>
    ViewModelBase? CurrentViewModel { get; }

    /// <summary>The page enum matching <see cref="CurrentViewModel"/>, handy for Active-state styling.</summary>
    AppPage CurrentPage { get; }

    /// <summary>Command form of <see cref="NavigateTo"/>, for binding directly to nav buttons.</summary>
    IRelayCommand<AppPage> NavigateCommand { get; }

    /// <summary>Registers how to build the view-model for a given page. Call once at startup per page.</summary>
    void Register(AppPage page, Func<ViewModelBase> factory, bool cache = true);

    /// <summary>Switches the current page, building (or reusing) its view-model.</summary>
    void NavigateTo(AppPage page);
}