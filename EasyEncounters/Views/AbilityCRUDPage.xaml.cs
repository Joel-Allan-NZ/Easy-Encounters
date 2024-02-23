using EasyEncounters.ViewModels;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EasyEncounters.Views;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class AbilityCRUDPage : Page
{
    public AbilityCRUDPage()
    {
        ViewModel = App.GetService<AbilityCRUDViewModel>();
        //ViewModel.Sorting += Sorting;
        this.InitializeComponent();
    }

    public AbilityCRUDViewModel ViewModel
    {
        get;
    }

    //private void Sorting(object sender, DataGridColumnEventArgs e)
    //{
    //    foreach (var dgColumn in AbilityListDG.Columns)
    //    {
    //        if (dgColumn.Tag.ToString() != e.Column.Tag.ToString())
    //        {
    //            dgColumn.SortDirection = null;
    //        }

    //    }
    //}
}