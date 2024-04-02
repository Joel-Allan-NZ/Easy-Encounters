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
public sealed partial class PartyDataGrid : UserControl
{
    public PartyDataGrid()
    {
        this.InitializeComponent();
    }

    // Using a DependencyProperty as the backing store for CopyPartyCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CopyPartyCommandProperty =
        DependencyProperty.Register("CopyPartyCommand", typeof(ICommand), typeof(PartyGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for DataGridSource.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DataGridSourceProperty =
        DependencyProperty.Register("DataGridSource", typeof(IEnumerable), typeof(PartyGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for DeletePartyCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DeletePartyCommandProperty =
        DependencyProperty.Register("DeletePartyCommand", typeof(ICommand), typeof(PartyGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for EditPartyCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty EditPartyCommandProperty =
        DependencyProperty.Register("EditPartyCommand", typeof(ICommand), typeof(PartyGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for SelectPartyCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SelectPartyCommandProperty =
        DependencyProperty.Register("SelectPartyCommand", typeof(ICommand), typeof(PartyGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for SortCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SortCommandProperty =
        DependencyProperty.Register("SortCommand", typeof(ICommand), typeof(PartyGrid), new PropertyMetadata(null));

    public ICommand CopyPartyCommand
    {
        get => (ICommand)GetValue(CopyPartyCommandProperty);
        set => SetValue(CopyPartyCommandProperty, value);
    }

    public IEnumerable DataGridSource
    {
        get => (IEnumerable)GetValue(DataGridSourceProperty);
        set => SetValue(DataGridSourceProperty, value);
    }

    public ICommand DeletePartyCommand
    {
        get => (ICommand)GetValue(DeletePartyCommandProperty);
        set => SetValue(DeletePartyCommandProperty, value);
    }

    public ICommand EditPartyCommand
    {
        get => (ICommand)GetValue(EditPartyCommandProperty);
        set => SetValue(EditPartyCommandProperty, value);
    }

    public ICommand SelectPartyCommand
    {
        get => (ICommand)GetValue(SelectPartyCommandProperty);
        set => SetValue(SelectPartyCommandProperty, value);
    }

    public ICommand SortCommand
    {
        get => (ICommand)GetValue(SortCommandProperty);
        set => SetValue(SortCommandProperty, value);
    }


    private void PartyDG_Sorting(object sender, CommunityToolkit.WinUI.UI.Controls.DataGridColumnEventArgs e)
    {
        foreach (var dgColumn in PartyDG.Columns)
        {
            if (dgColumn.Tag.ToString() != e.Column.Tag.ToString())
            {
                dgColumn.SortDirection = null;
            }
        }
    }
}
