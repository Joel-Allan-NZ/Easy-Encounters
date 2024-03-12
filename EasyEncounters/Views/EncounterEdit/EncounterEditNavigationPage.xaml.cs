using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using EasyEncounters.Views.CreatureEdit;
using EasyEncounters.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EasyEncounters.Views.EncounterEdit;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class EncounterEditNavigationPage : Page
{
    public EncounterEditNavigationViewModel ViewModel
    {
        get; private set;
    }

    public EncounterEditNavigationPage()
    {
        ViewModel = App.GetService<EncounterEditNavigationViewModel>();
        this.InitializeComponent();
    }
    private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        var navOptions = new FrameNavigationOptions
        {
            TransitionInfoOverride = args.RecommendedNavigationTransitionInfo,
            IsNavigationStackEnabled = false,
        };

        switch (args.InvokedItemContainer.Name)
        {
            case nameof(BasicInfoContentEncounter):
                ContentFrame.NavigateToType(typeof(BasicInfoContentEncounterPage), null, navOptions);
                break;

            case nameof(PartyAndDifficulty):
                ContentFrame.NavigateToType(typeof(PartyAndDifficultyPage), null, navOptions);
                break;

            case nameof(Creatures):
                ContentFrame.NavigateToType(typeof(CreaturesPage), null, navOptions);
                break;
        }
    }

    private void NavView_Loaded(object sender, RoutedEventArgs e)
    {
        var navOptions = new FrameNavigationOptions
        {
            IsNavigationStackEnabled = false,
        };
        rootNavigationView.SelectedItem = rootNavigationView.MenuItems[0];
        ContentFrame.NavigateToType(typeof(BasicInfoContentEncounterPage), null, navOptions);
    }
}



