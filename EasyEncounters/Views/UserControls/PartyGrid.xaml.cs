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

    // Using a DependencyProperty as the backing store for FirstAsyncCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FirstAsyncCommandProperty =
        DependencyProperty.Register("FirstAsyncCommand", typeof(ICommand), typeof(EncounterGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for LastAsyncCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty LastAsyncCommandProperty =
        DependencyProperty.Register("LastAsyncCommand", typeof(ICommand), typeof(EncounterGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for NextAsyncCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty NextAsyncCommandProperty =
        DependencyProperty.Register("NextAsyncCommand", typeof(ICommand), typeof(EncounterGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for PageCount.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty PageCountProperty =
        DependencyProperty.Register("PageCount", typeof(int), typeof(EncounterGrid), new PropertyMetadata(0));

    // Using a DependencyProperty as the backing store for PageNumber.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty PageNumberProperty =
        DependencyProperty.Register("PageNumber", typeof(int), typeof(EncounterGrid), new PropertyMetadata(0));

    // Using a DependencyProperty as the backing store for PreviousAsyncCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty PreviousAsyncCommandProperty =
        DependencyProperty.Register("PreviousAsyncCommand", typeof(ICommand), typeof(EncounterGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for SearchString.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SearchStringProperty =
        DependencyProperty.Register("SearchString", typeof(string), typeof(EncounterGrid), new PropertyMetadata(""));

    // Using a DependencyProperty as the backing store for SearchTextChangeCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SearchTextChangeCommandProperty =
        DependencyProperty.Register("SearchTextChangeCommand", typeof(ICommand), typeof(PartyGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for SelectPartyCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SelectPartyCommandProperty =
        DependencyProperty.Register("SelectPartyCommand", typeof(ICommand), typeof(PartyGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for SortCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SortCommandProperty =
        DependencyProperty.Register("SortCommand", typeof(ICommand), typeof(PartyGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for SuggestionChosenCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SuggestionChosenCommandProperty =
        DependencyProperty.Register("SuggestionChosenCommand", typeof(ICommand), typeof(EncounterGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for Suggestions.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SuggestionsProperty =
        DependencyProperty.Register("Suggestions", typeof(object), typeof(PartyGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for TitleText.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TitleTextProperty =
        DependencyProperty.Register("TitleText", typeof(string), typeof(PartyGrid), new PropertyMetadata(""));

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

    public ICommand FirstAsyncCommand
    {
        get => (ICommand)GetValue(FirstAsyncCommandProperty);
        set => SetValue(FirstAsyncCommandProperty, value);
    }

    public ICommand LastAsyncCommand
    {
        get => (ICommand)GetValue(LastAsyncCommandProperty);
        set => SetValue(LastAsyncCommandProperty, value);
    }

    public ICommand NextAsyncCommand
    {
        get => (ICommand)GetValue(NextAsyncCommandProperty);
        set => SetValue(NextAsyncCommandProperty, value);
    }

    public int PageCount
    {
        get => (int)GetValue(PageCountProperty);
        set => SetValue(PageCountProperty, value);
    }

    public int PageNumber
    {
        get => (int)GetValue(PageNumberProperty);
        set => SetValue(PageNumberProperty, value);
    }

    public ICommand PreviousAsyncCommand
    {
        get => (ICommand)GetValue(PreviousAsyncCommandProperty);
        set => SetValue(PreviousAsyncCommandProperty, value);
    }

    public string SearchString
    {
        get => (string)GetValue(SearchStringProperty);
        set => SetValue(SearchStringProperty, value);
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

    public ICommand SuggestionChosenCommand
    {
        get => (ICommand)GetValue(SuggestionChosenCommandProperty);
        set => SetValue(SuggestionChosenCommandProperty, value);
    }

    public object Suggestions
    {
        get => (object)GetValue(SuggestionsProperty);
        set => SetValue(SuggestionsProperty, value);
    }

    public string TitleText
    {
        get => (string)GetValue(TitleTextProperty);
        set => SetValue(TitleTextProperty, value);
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