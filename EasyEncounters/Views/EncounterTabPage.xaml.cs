using EasyEncounters.ViewModels;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EasyEncounters.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class EncounterTabPage : Page
{
    public EncounterTabPage()
    {
        ViewModel = App.GetService<EncounterTabViewModel>();
        this.InitializeComponent();
    }

    public EncounterTabViewModel ViewModel
    {
        get;
    }

    private void TabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
    {
        ViewModel.CloseTabCommand.Execute(args.Tab.DataContext); //bit of a hack, but quick and less messy than the alternative.
    }
}