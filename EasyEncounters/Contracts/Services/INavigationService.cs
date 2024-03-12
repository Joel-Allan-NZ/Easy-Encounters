﻿using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace EasyEncounters.Contracts.Services;

public interface INavigationService
{
    event NavigatedEventHandler Navigated;

    bool CanGoBack
    {
        get;
    }

    Frame? Frame
    {
        get; set;
    }

    nint GetWindowHandle();

    bool GoBack();

    bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false, bool ignoreNavigation = false);


}