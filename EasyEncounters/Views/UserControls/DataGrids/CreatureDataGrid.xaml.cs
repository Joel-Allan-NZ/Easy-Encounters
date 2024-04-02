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
public sealed partial class CreatureDataGrid : UserControl
{
    public CreatureDataGrid()
    {
        this.InitializeComponent();
    }

    public ICommand CopyCreatureCommand
    {
        get => (ICommand)GetValue(CopyCreatureCommandProperty);
        set => SetValue(CopyCreatureCommandProperty, value);
    }

    public ICommand DeleteCreatureCommand
    {
        get => (ICommand)GetValue(DeleteCreatureCommandProperty);
        set => SetValue(DeleteCreatureCommandProperty, value);
    }

    public ICommand EditCreatureCommand
    {
        get => (ICommand)GetValue(EditCreatureCommandProperty);
        set => SetValue(EditCreatureCommandProperty, value);
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

    public ICommand SelectCreatureCommand
    {
        get => (ICommand)GetValue(SelectCreatureCommandProperty);
        set => SetValue(SelectCreatureCommandProperty, value);
    }

    public ICommand SortCommand
    {
        get => (ICommand)GetValue(SortCommandProperty);
        set => SetValue(SortCommandProperty, value);
    }

    // Using a DependencyProperty as the backing store for CopyCreatureCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CopyCreatureCommandProperty =
        DependencyProperty.Register("CopyCreatureCommand", typeof(ICommand), typeof(CreatureGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for DeleteCreatureCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DeleteCreatureCommandProperty =
        DependencyProperty.Register("DeleteCreatureCommand", typeof(ICommand), typeof(CreatureGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for EditCreatureCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty EditCreatureCommandProperty =
        DependencyProperty.Register("EditCreatureCommand", typeof(ICommand), typeof(CreatureGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for GridDataSource.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty GridDataSourceProperty =
        DependencyProperty.Register("GridDataSource", typeof(IEnumerable), typeof(CreatureGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for InteractableRows.  This enables animation, styling, binding, etc...s
    public static readonly DependencyProperty InteractableRowsProperty =
        DependencyProperty.Register("InteractableRows", typeof(bool), typeof(CreatureGrid), new PropertyMetadata(false));

    // Using a DependencyProperty as the backing store for SelectCreatureCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SelectCreatureCommandProperty =
        DependencyProperty.Register("SelectCreatureCommand", typeof(ICommand), typeof(CreatureGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for SortCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SortCommandProperty =
        DependencyProperty.Register("SortCommand", typeof(ICommand), typeof(CreatureGrid), new PropertyMetadata(null));

    private void CreatureDG_Sorting(object sender, CommunityToolkit.WinUI.UI.Controls.DataGridColumnEventArgs e)
    {
        foreach (var dgColumn in CreatureDG.Columns)
        {
            if (dgColumn.Tag.ToString() != e.Column.Tag.ToString())
            {
                dgColumn.SortDirection = null;
            }
        }
    }
}
