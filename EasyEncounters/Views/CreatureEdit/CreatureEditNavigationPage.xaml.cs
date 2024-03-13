using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using EasyEncounters.ViewModels;
using EasyEncounters.Views.CreatureEdit;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Windows.Devices.PointOfService;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EasyEncounters.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class CreatureEditNavigationPage : Page
{
    public CreatureEditNavigationPageViewModel ViewModel
    {
    get; private set; }
    public CreatureEditNavigationPage()
    {
        ViewModel = App.GetService<CreatureEditNavigationPageViewModel>();

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
            case nameof(BasicInfoContent):
                ContentFrame.NavigateToType(typeof(BasicInfoPage), null, navOptions);
                break;

            case nameof(CoreStatsContent):
                ContentFrame.NavigateToType(typeof(CoreStatsPage), null, navOptions);
                break;

            case nameof(OptionalStatsContent):
                ContentFrame.NavigateToType(typeof(OptionalStatsPage), null, navOptions);
                break;

            case nameof(SkillsContent):
                ContentFrame.NavigateToType(typeof(SkillsPage), null, navOptions);
                break;

            case nameof(AttacksAndAbilitiesContent):
                ContentFrame.NavigateToType(typeof(AttacksAndAbilitiesPage), null, navOptions);
                break;

            case nameof(CRAdviceContent):
                ContentFrame.NavigateToType(typeof(DMCRAdvicePage), null, navOptions);
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
        ContentFrame.NavigateToType(typeof(BasicInfoPage), null, navOptions);
    }

}
