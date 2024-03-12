using System.Collections;
using System.Windows.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EasyEncounters.Views.UserControls;

public sealed partial class PartyGrid : UserControl
{
    // Using a DependencyProperty as the backing store for AddNewPartyCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty AddNewPartyCommandProperty =
        DependencyProperty.Register("AddNewPartyCommand", typeof(ICommand), typeof(PartyGrid), new PropertyMetadata(null));

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

    // Using a DependencyProperty as the backing store for FilterCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterCommandProperty =
        DependencyProperty.Register("FilterCommand", typeof(ICommand), typeof(PartyGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for SearchTextChangeCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SearchTextChangeCommandProperty =
        DependencyProperty.Register("SearchTextChangeCommand", typeof(ICommand), typeof(PartyGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for SelectPartyCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SelectPartyCommandProperty =
        DependencyProperty.Register("SelectPartyCommand", typeof(ICommand), typeof(PartyGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for SortCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SortCommandProperty =
        DependencyProperty.Register("SortCommand", typeof(ICommand), typeof(PartyGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for Suggestions.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SuggestionsProperty =
        DependencyProperty.Register("Suggestions", typeof(object), typeof(PartyGrid), new PropertyMetadata(null));

    public PartyGrid()
    {
        this.InitializeComponent();
    }

    public ICommand AddNewPartyCommand
    {
        get => (ICommand)GetValue(AddNewPartyCommandProperty);
        set => SetValue(AddNewPartyCommandProperty, value);
    }

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

    public ICommand FilterCommand
    {
        get => (ICommand)GetValue(FilterCommandProperty);
        set => SetValue(FilterCommandProperty, value);
    }

    public ICommand SearchTextChangeCommand
    {
        get => (ICommand)GetValue(SearchTextChangeCommandProperty);
        set => SetValue(SearchTextChangeCommandProperty, value);
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

    public object Suggestions
    {
        get => (object)GetValue(SuggestionsProperty);
        set => SetValue(SuggestionsProperty, value);
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