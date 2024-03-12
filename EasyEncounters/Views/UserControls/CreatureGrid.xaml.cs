using System.Collections;
using System.Windows.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EasyEncounters.Views.UserControls;

public sealed partial class CreatureGrid : UserControl
{
    // Using a DependencyProperty as the backing store for AddNewCreatureCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty AddNewCreatureCommandProperty =
        DependencyProperty.Register("AddNewCreatureCommand", typeof(ICommand), typeof(CreatureGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for ClearFiltersCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ClearFiltersCommandProperty =
        DependencyProperty.Register("ClearFiltersCommand", typeof(ICommand), typeof(CreatureGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for CopyCreatureCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CopyCreatureCommandProperty =
        DependencyProperty.Register("CopyCreatureCommand", typeof(ICommand), typeof(CreatureGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for DeleteCreatureCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DeleteCreatureCommandProperty =
        DependencyProperty.Register("DeleteCreatureCommand", typeof(ICommand), typeof(CreatureGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for EditCreatureCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty EditCreatureCommandProperty =
        DependencyProperty.Register("EditCreatureCommand", typeof(ICommand), typeof(CreatureGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterAlignmentSelected.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterAlignmentSelectedProperty =
        DependencyProperty.Register("FilterAlignmentSelected", typeof(object), typeof(CreatureGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterAlignmentSource.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterAlignmentSourceProperty =
        DependencyProperty.Register("FilterAlignmentSource", typeof(object), typeof(CreatureGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterCommandProperty =
        DependencyProperty.Register("FilterCommand", typeof(ICommand), typeof(CreatureGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterCreatureTypeSelected.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterCreatureTypeSelectedProperty =
        DependencyProperty.Register("FilterCreatureTypeSelected", typeof(object), typeof(CreatureGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterCreatureTypeSource.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterCreatureTypeSourceProperty =
        DependencyProperty.Register("FilterCreatureTypeSource", typeof(object), typeof(CreatureGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterMaximumCR.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterMaximumCRProperty =
        DependencyProperty.Register("FilterMaximumCR", typeof(double), typeof(CreatureGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterMaximumSize.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterMaximumSizeProperty =
        DependencyProperty.Register("FilterMaximumSize", typeof(object), typeof(CreatureGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterMinimumCR.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterMinimumCRProperty =
        DependencyProperty.Register("FilterMinimumCR", typeof(double), typeof(CreatureGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterMinimumSize.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterMinimumSizeProperty =
        DependencyProperty.Register("FilterMinimumSize", typeof(object), typeof(CreatureGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterSizeSource.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterSizeSourceProperty =
        DependencyProperty.Register("FilterSizeSource", typeof(object), typeof(CreatureGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for GridDataSource.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty GridDataSourceProperty =
        DependencyProperty.Register("GridDataSource", typeof(IEnumerable), typeof(CreatureGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for SearchTextChangeCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SearchTextChangeCommandProperty =
        DependencyProperty.Register("SearchTextChangeCommand", typeof(ICommand), typeof(CreatureGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for SelectCreatureCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SelectCreatureCommandProperty =
        DependencyProperty.Register("SelectCreatureCommand", typeof(ICommand), typeof(CreatureGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for SortCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SortCommandProperty =
        DependencyProperty.Register("SortCommand", typeof(ICommand), typeof(CreatureGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for Suggestions.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SuggestionsProperty =
        DependencyProperty.Register("Suggestions", typeof(object), typeof(CreatureGrid), new PropertyMetadata(null));

    public CreatureGrid()
    {
        InitializeComponent();
    }

    public ICommand AddNewCreatureCommand
    {
        get => (ICommand)GetValue(AddNewCreatureCommandProperty);
        set => SetValue(AddNewCreatureCommandProperty, value);
    }

    public ICommand ClearFiltersCommand
    {
        get => (ICommand)GetValue(ClearFiltersCommandProperty);
        set => SetValue(ClearFiltersCommandProperty, value);
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

    public object FilterAlignmentSelected
    {
        get => (object)GetValue(FilterAlignmentSelectedProperty);
        set => SetValue(FilterAlignmentSelectedProperty, value);
    }

    public object FilterAlignmentSource
    {
        get => (object)GetValue(FilterAlignmentSourceProperty);
        set => SetValue(FilterAlignmentSourceProperty, value);
    }

    public ICommand FilterCommand
    {
        get => (ICommand)GetValue(FilterCommandProperty);
        set => SetValue(FilterCommandProperty, value);
    }

    public object FilterCreatureTypeSelected
    {
        get => GetValue(FilterCreatureTypeSelectedProperty);
        set => SetValue(FilterCreatureTypeSelectedProperty, value);
    }

    public object FilterCreatureTypeSource
    {
        get => (object)GetValue(FilterCreatureTypeSourceProperty);
        set => SetValue(FilterCreatureTypeSourceProperty, value);
    }

    public double FilterMaximumCR
    {
        get => (double)GetValue(FilterMaximumCRProperty);
        set => SetValue(FilterMaximumCRProperty, value);
    }

    public object FilterMaximumSize
    {
        get => (object)GetValue(FilterMaximumSizeProperty);
        set => SetValue(FilterMaximumSizeProperty, value);
    }

    public double FilterMinimumCR
    {
        get => (double)GetValue(FilterMinimumCRProperty);
        set => SetValue(FilterMinimumCRProperty, value);
    }

    public object FilterMinimumSize
    {
        get => (object)GetValue(FilterMinimumSizeProperty);
        set => SetValue(FilterMinimumSizeProperty, value);
    }

    public object FilterSizeSource
    {
        get => (object)GetValue(FilterSizeSourceProperty);
        set => SetValue(FilterSizeSourceProperty, value);
    }

    public IEnumerable GridDataSource
    {
        get => (IEnumerable)GetValue(GridDataSourceProperty);
        set => SetValue(GridDataSourceProperty, value);
    }

    public ICommand SearchTextChangeCommand
    {
        get => (ICommand)GetValue(SearchTextChangeCommandProperty);
        set => SetValue(SearchTextChangeCommandProperty, value);
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

    public object Suggestions
    {
        get => (object)GetValue(SuggestionsProperty);
        set => SetValue(SuggestionsProperty, value);
    }

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



    public bool InteractableRows
    {
        get => (bool)GetValue(InteractableRowsProperty);
        set => SetValue(InteractableRowsProperty, value);
    }

    // Using a DependencyProperty as the backing store for InteractableRows.  This enables animation, styling, binding, etc...s
    public static readonly DependencyProperty InteractableRowsProperty =
        DependencyProperty.Register("InteractableRows", typeof(bool), typeof(CreatureGrid), new PropertyMetadata(false));


}