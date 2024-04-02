using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
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

namespace EasyEncounters.Views.UserControls.DataGrids;
public sealed partial class EncounterDataGrid : UserControl
{
    public EncounterDataGrid()
    {
        this.InitializeComponent();
    }

    // Using a DependencyProperty as the backing store for CopyEncounterCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CopyEncounterCommandProperty =
        DependencyProperty.Register("CopyEncounterCommand", typeof(ICommand), typeof(EncounterGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for DeleteEncounterCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DeleteEncounterCommandProperty =
        DependencyProperty.Register("DeleteEncounterCommand", typeof(ICommand), typeof(EncounterGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for EditEncounterCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty EditEncounterCommandProperty =
        DependencyProperty.Register("EditEncounterCommand", typeof(ICommand), typeof(EncounterGrid), new PropertyMetadata(null));
    // Using a DependencyProperty as the backing store for GridDataSource.  This enables animation, styling, binding, etc...

    public static readonly DependencyProperty GridDataSourceProperty =
        DependencyProperty.Register("GridDataSource", typeof(IEnumerable), typeof(EncounterGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for InteractableRows.  This enables animation, styling, binding, etc...s
    public static readonly DependencyProperty InteractableRowsProperty =
        DependencyProperty.Register("InteractableRows", typeof(bool), typeof(EncounterGrid), new PropertyMetadata(false));

    // Using a DependencyProperty as the backing store for SortCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SortCommandProperty =
        DependencyProperty.Register("SortCommand", typeof(ICommand), typeof(EncounterGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for SelectEncounterCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SelectEncounterCommandProperty =
        DependencyProperty.Register("SelectEncounterCommand", typeof(ICommand), typeof(EncounterGrid), new PropertyMetadata(null));
    public ICommand CopyEncounterCommand
    {
        get => (ICommand)GetValue(CopyEncounterCommandProperty);
        set => SetValue(CopyEncounterCommandProperty, value);
    }

    public ICommand DeleteEncounterCommand
    {
        get => (ICommand)GetValue(DeleteEncounterCommandProperty);
        set => SetValue(DeleteEncounterCommandProperty, value);
    }

    public ICommand EditEncounterCommand
    {
        get => (ICommand)GetValue(EditEncounterCommandProperty);
        set => SetValue(EditEncounterCommandProperty, value);
    }

    public IEnumerable GridDataSource
    {
        get => (IEnumerable)GetValue(GridDataSourceProperty);
        set => SetValue(GridDataSourceProperty, value);
    }


    public bool InteractableRows
    {
        get => (bool)GetValue(InteractableRowsProperty);
        set => SetValue(InteractableRowsProperty, value);
    }

    public ICommand SelectEncounterCommand
    {
        get => (ICommand)GetValue(SelectEncounterCommandProperty);
        set => SetValue(SelectEncounterCommandProperty, value);
    }

    public ICommand SortCommand
    {
        get => (ICommand)GetValue(SortCommandProperty);
        set => SetValue(SortCommandProperty, value);
    }

    private void EncounterDG_Sorting(object sender, CommunityToolkit.WinUI.UI.Controls.DataGridColumnEventArgs e)
    {
        foreach (var dgColumn in EncounterDG.Columns)
        {
            if (dgColumn.Tag.ToString() != e.Column.Tag.ToString())
            {
                dgColumn.SortDirection = null;
            }
        }
    }
}
