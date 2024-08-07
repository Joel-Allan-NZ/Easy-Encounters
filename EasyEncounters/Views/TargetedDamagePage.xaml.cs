using System.Text.RegularExpressions;
using EasyEncounters.ViewModels;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EasyEncounters.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class TargetedDamagePage : Page
{
    public TargetedDamagePage()
    {
        ViewModel = App.GetService<TargetedDamageViewModel>();
        this.InitializeComponent();
    }

    public TargetedDamageViewModel ViewModel
    {
        get;
    }

    private void OnTextChanging(object sender, TextBoxTextChangingEventArgs e)
    {
        // Get the current text of the TextBox
        var text = ((TextBox)sender).Text;

        // Use a regular expression to only allow numeric values
        var regex = new Regex("^[0-9]*$");

        // If the text does not match the regular expression, undo the change
        if (!regex.IsMatch(text))
        {
            ((TextBox)sender).Undo();
        }
    }
}