using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using CommunityToolkit.WinUI.UI.Controls;
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
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class AbilityCRUDPage : Page
{
    public AbilityCRUDViewModel ViewModel
    {
        get;
    }
    public AbilityCRUDPage()
    {
        ViewModel = App.GetService<AbilityCRUDViewModel>();
        ViewModel.Sorting += Sorting;
        this.InitializeComponent();
    }
    private void Sorting(object sender, DataGridColumnEventArgs e)
    {
        foreach (var dgColumn in AbilityListDG.Columns)
        {
            if (dgColumn.Tag.ToString() != e.Column.Tag.ToString())
            {
                dgColumn.SortDirection = null;
            }

        }
    }
}
