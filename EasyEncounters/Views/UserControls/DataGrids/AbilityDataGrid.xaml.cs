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
public sealed partial class AbilityDataGrid : UserControl
{
    public AbilityDataGrid()
    {
        this.InitializeComponent();
    }

    // Using a DependencyProperty as the backing store for CopySpellCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CopySpellCommandProperty =
        DependencyProperty.Register("CopySpellCommand", typeof(ICommand), typeof(SpellGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for DeleteSpellCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DeleteSpellCommandProperty =
        DependencyProperty.Register("DeleteSpellCommand", typeof(ICommand), typeof(SpellGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for EditSpellCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty EditSpellCommandProperty =
        DependencyProperty.Register("EditSpellCommand", typeof(ICommand), typeof(SpellGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for GridDataSource.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty GridDataSourceProperty =
        DependencyProperty.Register("GridDataSource", typeof(IEnumerable), typeof(SpellGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for SelectSpellCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SelectSpellCommandProperty =
        DependencyProperty.Register("SelectSpellCommand", typeof(ICommand), typeof(SpellGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SortCommandProperty =
        DependencyProperty.Register("SortCommand", typeof(ICommand), typeof(SpellGrid), new PropertyMetadata(null));

    public ICommand CopySpellCommand
    {
        get => (ICommand)GetValue(CopySpellCommandProperty);
        set => SetValue(CopySpellCommandProperty, value);
    }

    public ICommand DeleteSpellCommand
    {
        get => (ICommand)GetValue(DeleteSpellCommandProperty);
        set => SetValue(DeleteSpellCommandProperty, value);
    }

    public ICommand EditSpellCommand
    {
        get => (ICommand)GetValue(EditSpellCommandProperty);
        set => SetValue(EditSpellCommandProperty, value);
    }

    public IEnumerable GridDataSource
    {
        get => (IEnumerable)GetValue(GridDataSourceProperty);
        set => SetValue(GridDataSourceProperty, value);
    }

    public ICommand SelectSpellCommand
    {
        get => (ICommand)GetValue(SelectSpellCommandProperty);
        set => SetValue(SelectSpellCommandProperty, value);
    }

    public ICommand SortCommand
    {
        get => (ICommand)GetValue(SortCommandProperty);
        set => SetValue(SortCommandProperty, value);
    }

    private void SpellDG_Sorting(object sender, CommunityToolkit.WinUI.UI.Controls.DataGridColumnEventArgs e)
    {
        foreach (var dgColumn in SpellDG.Columns)
        {
            if (dgColumn.Tag.ToString() != e.Column.Tag.ToString())
            {
                dgColumn.SortDirection = null;
            }
        }
    }
}
