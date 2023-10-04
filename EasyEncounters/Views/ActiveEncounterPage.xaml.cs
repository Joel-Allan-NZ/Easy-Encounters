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
using EasyEncounters.ViewModels;
using System.Text.RegularExpressions;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EasyEncounters.Views
{
    public sealed partial class ActiveEncounterPage : Page
    {
        public ActiveEncounterViewModel ViewModel
        {
            get;
        }

        public ActiveEncounterPage()
        {
            ViewModel = App.GetService<ActiveEncounterViewModel>();
            this.InitializeComponent();
        }

        //todo: Proper validation rather than this
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

}
