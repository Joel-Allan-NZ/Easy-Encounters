using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using EasyEncounters.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EasyEncounters.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class CreatureEditPage : Page
{
    public CreatureEditViewModel ViewModel
    {
        get;
    }
    public CreatureEditPage()
    {
        ViewModel = App.GetService<CreatureEditViewModel>();
        this.InitializeComponent();
    }

    private void OnTextChanging(object sender, TextBoxTextChangingEventArgs e)
    {
        // Get the current text of the TextBox
        var text = ((TextBox)sender).Text;

        // Use a regular expression to only allow numeric values
        var regex = new Regex("^[0-9-]*$");

        // If the text does not match the regular expression, undo the change
        if (!regex.IsMatch(text))
        {
            ((TextBox)sender).Undo();
        }
        if (text.Length == 0)
        {
            ((TextBox)sender).SelectedText = "0";
        }
    }

    private void OnTextChangingDouble(object sender, TextBoxTextChangingEventArgs e)
    {
        // Get the current text of the TextBox
        var text = ((TextBox)sender).Text;

        // Use a regular expression to only allow numeric values
        var regex = new Regex("^[0-9.-]*$");

        // If the text does not match the regular expression, undo the change
        if (!regex.IsMatch(text))
        {
            ((TextBox)sender).Undo();
        }
        if (text.Length == 0)
        {
            ((TextBox)sender).SelectedText = "0";

        }
            
    }
}
