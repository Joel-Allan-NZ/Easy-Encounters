using CommunityToolkit.WinUI.UI.Controls;
using EasyEncounters.ViewModels;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EasyEncounters.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class EncounterSelectPage : Page
{
    public EncounterSelectPage()
    {
        ViewModel = App.GetService<EncounterSelectViewModel>();
        InitializeComponent();
    }

    public EncounterSelectViewModel ViewModel
    {
        get;
    }

}