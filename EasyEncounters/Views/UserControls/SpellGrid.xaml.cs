using System.Collections;
using System.Windows.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EasyEncounters.Views.UserControls;

public sealed partial class SpellGrid : UserControl
{
    // Using a DependencyProperty as the backing store for AddSpellCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty AddSpellCommandProperty =
        DependencyProperty.Register("AddSpellCommand", typeof(ICommand), typeof(SpellGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for ClearFiltersCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ClearFiltersCommandProperty =
        DependencyProperty.Register("ClearFiltersCommand", typeof(ICommand), typeof(SpellGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for CopySpellCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CopySpellCommandProperty =
        DependencyProperty.Register("CopySpellCommand", typeof(ICommand), typeof(SpellGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for DeleteSpellCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DeleteSpellCommandProperty =
        DependencyProperty.Register("DeleteSpellCommand", typeof(ICommand), typeof(SpellGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for EditSpellCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty EditSpellCommandProperty =
        DependencyProperty.Register("EditSpellCommand", typeof(ICommand), typeof(SpellGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterActionSpeedSelectedProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterActionSpeedSelectedProperty =
        DependencyProperty.Register("FilterActionSpeedSelected", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterActionSpeedSource.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterActionSpeedSourceProperty =
        DependencyProperty.Register("FilterActionSpeedSource", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterCommandProperty =
        DependencyProperty.Register("FilterCommand", typeof(ICommand), typeof(SpellGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterConcentrationSelected.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterConcentrationSelectedProperty =
        DependencyProperty.Register("FilterConcentrationSelected", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterConcentrationSource.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterConcentrationSourceProperty =
        DependencyProperty.Register("FilterConcentrationSource", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterDamageTypeSelected.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterDamageTypeSelectedProperty =
        DependencyProperty.Register("FilterDamageTypeSelected", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterDamageTypeSource.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterDamageTypeSourceProperty =
        DependencyProperty.Register("FilterDamageTypeSource", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterMaximumSpellLevel.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterMaximumSpellLevelProperty =
        DependencyProperty.Register("FilterMaximumSpellLevel", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterMinimumSpellLevel.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterMinimumSpellLevelProperty =
        DependencyProperty.Register("FilterMinimumSpellLevel", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterResolutionSelected.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterResolutionSelectedProperty =
        DependencyProperty.Register("FilterResolutionSelected", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterResolutionSource.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterResolutionSourceProperty =
        DependencyProperty.Register("FilterResolutionSource", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterSpellLevelSource.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterSpellLevelSourceProperty =
        DependencyProperty.Register("FilterSpellLevelSource", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterSpellSchoolSelected.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterSpellSchoolSelectedProperty =
        DependencyProperty.Register("FilterSpellSchoolSelected", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterSpellSchoolSource.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterSpellSchoolSourceProperty =
        DependencyProperty.Register("FilterSpellSchoolSource", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FirstAsyncCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FirstAsyncCommandProperty =
        DependencyProperty.Register("FirstAsyncCommand", typeof(ICommand), typeof(EncounterGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for GridDataSource.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty GridDataSourceProperty =
        DependencyProperty.Register("GridDataSource", typeof(IEnumerable), typeof(SpellGrid), new PropertyMetadata(null));

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
        DependencyProperty.Register("SearchTextChangeCommand", typeof(ICommand), typeof(SpellGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for SelectSpellCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SelectSpellCommandProperty =
        DependencyProperty.Register("SelectSpellCommand", typeof(ICommand), typeof(SpellGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SortCommandProperty =
        DependencyProperty.Register("SortCommand", typeof(ICommand), typeof(SpellGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for SuggestionChosenCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SuggestionChosenCommandProperty =
        DependencyProperty.Register("SuggestionChosenCommand", typeof(ICommand), typeof(EncounterGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for Suggestions.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SuggestionsProperty =
        DependencyProperty.Register("Suggestions", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for TitleText.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TitleTextProperty =
        DependencyProperty.Register("TitleText", typeof(string), typeof(SpellGrid), new PropertyMetadata(""));

    public SpellGrid()
    {
        InitializeComponent();
    }

    public ICommand AddSpellCommand
    {
        get => (ICommand)GetValue(AddSpellCommandProperty);
        set => SetValue(AddSpellCommandProperty, value);
    }

    public ICommand ClearFiltersCommand
    {
        get => (ICommand)GetValue(ClearFiltersCommandProperty);
        set => SetValue(ClearFiltersCommandProperty, value);
    }

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

    public object FilterActionSpeedSelected
    {
        get => (object)GetValue(FilterActionSpeedSelectedProperty);
        set => SetValue(FilterActionSpeedSelectedProperty, value);
    }

    public object FilterActionSpeedSource
    {
        get => (object)GetValue(FilterActionSpeedSourceProperty);
        set => SetValue(FilterActionSpeedSourceProperty, value);
    }

    public ICommand FilterCommand
    {
        get => (ICommand)GetValue(FilterCommandProperty);
        set => SetValue(FilterCommandProperty, value);
    }

    public object FilterConcentrationSelected
    {
        get => GetValue(FilterConcentrationSelectedProperty);
        set => SetValue(FilterConcentrationSelectedProperty, value);
    }

    public object FilterConcentrationSource
    {
        get => GetValue(FilterConcentrationSourceProperty);
        set => SetValue(FilterConcentrationSourceProperty, value);
    }

    public object FilterDamageTypeSelected
    {
        get => (object)GetValue(FilterDamageTypeSelectedProperty);
        set => SetValue(FilterDamageTypeSelectedProperty, value);
    }

    public object FilterDamageTypeSource
    {
        get => GetValue(FilterDamageTypeSourceProperty);
        set => SetValue(FilterDamageTypeSourceProperty, value);
    }

    public object FilterMaximumSpellLevel
    {
        get => (object)GetValue(FilterMaximumSpellLevelProperty);
        set => SetValue(FilterMaximumSpellLevelProperty, value);
    }

    public object FilterMinimumSpellLevel
    {
        get => (object)GetValue(FilterMinimumSpellLevelProperty);
        set => SetValue(FilterMinimumSpellLevelProperty, value);
    }

    public object FilterResolutionSelected
    {
        get => (object)GetValue(FilterResolutionSelectedProperty);
        set => SetValue(FilterResolutionSelectedProperty, value);
    }

    public object FilterResolutionSource
    {
        get
        {
            var filterResolutionSourceProperty = FilterResolutionSourceProperty;
            return (object)GetValue(filterResolutionSourceProperty);
        }
        set => SetValue(FilterResolutionSourceProperty, value);
    }

    public object FilterSpellLevelSource
    {
        get => (object)GetValue(FilterSpellLevelSourceProperty);
        set => SetValue(FilterSpellLevelSourceProperty, value);
    }

    public object FilterSpellSchoolSelected
    {
        get => (object)GetValue(FilterSpellSchoolSelectedProperty);
        set => SetValue(FilterSpellSchoolSelectedProperty, value);
    }

    public object FilterSpellSchoolSource
    {
        get => (object)GetValue(FilterSpellSchoolSourceProperty);
        set => SetValue(FilterSpellSchoolSourceProperty, value);
    }

    public ICommand FirstAsyncCommand
    {
        get => (ICommand)GetValue(FirstAsyncCommandProperty);
        set => SetValue(FirstAsyncCommandProperty, value);
    }

    public IEnumerable GridDataSource
    {
        get => (IEnumerable)GetValue(GridDataSourceProperty);
        set => SetValue(GridDataSourceProperty, value);
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