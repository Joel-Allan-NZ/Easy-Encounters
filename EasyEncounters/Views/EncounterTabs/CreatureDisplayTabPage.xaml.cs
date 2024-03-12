using System.Drawing;
using EasyEncounters.ViewModels;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EasyEncounters.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class CreatureDisplayTabPage : Page
{
    public CreatureDisplayTabPage()
    {
        ViewModel = App.GetService<CreatureDisplayTabViewModel>();
        this.InitializeComponent();
    }

    public CreatureDisplayTabViewModel ViewModel
    {
        get;
    }

    private void RichEditBox_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (!richFeaturesBox.IsReadOnly)
        {
            richFeaturesBox.Document.SetText(Microsoft.UI.Text.TextSetOptions.FormatRtf, ViewModel.Creature.Features);            
            richFeaturesBox.IsReadOnly = true;
        }
        
    }
}