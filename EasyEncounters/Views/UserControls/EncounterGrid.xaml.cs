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

namespace EasyEncounters.Views.UserControls;

public sealed partial class EncounterGrid : UserControl
{
    // Using a DependencyProperty as the backing store for AddNewEncounterCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty AddNewEncounterCommandProperty =
        DependencyProperty.Register("AddNewEncounterCommand", typeof(ICommand), typeof(EncounterGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for ClearFiltersCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ClearFiltersCommandProperty =
        DependencyProperty.Register("ClearFiltersCommand", typeof(ICommand), typeof(EncounterGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for CopyEncounterCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CopyEncounterCommandProperty =
        DependencyProperty.Register("CopyEncounterCommand", typeof(ICommand), typeof(EncounterGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for DeleteEncounterCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DeleteEncounterCommandProperty =
        DependencyProperty.Register("DeleteEncounterCommand", typeof(ICommand), typeof(EncounterGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for EditEncounterCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty EditEncounterCommandProperty =
        DependencyProperty.Register("EditEncounterCommand", typeof(ICommand), typeof(EncounterGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterCampaignSelected.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterCampaignSelectedProperty =
        DependencyProperty.Register("FilterCampaignSelected", typeof(object), typeof(EncounterGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterCampaignSource.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterCampaignSourceProperty =
        DependencyProperty.Register("FilterCampaignSource", typeof(object), typeof(EncounterGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterCommandProperty =
        DependencyProperty.Register("FilterCommand", typeof(ICommand), typeof(EncounterGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterEncounterDifficultySource.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterEncounterDifficultySourceProperty =
        DependencyProperty.Register("FilterEncounterDifficultySource", typeof(object), typeof(EncounterGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterMaximumEncounterDifficulty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterMaximumEncounterDifficultyProperty =
        DependencyProperty.Register("FilterMaximumEncounterDifficulty", typeof(object), typeof(EncounterGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterMaximumEnemies.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterMaximumEnemiesProperty =
        DependencyProperty.Register("FilterMaximumEnemies", typeof(double), typeof(EncounterGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterMinimumEncounterDifficulty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterMinimumEncounterDifficultyProperty =
        DependencyProperty.Register("FilterMinimumEncounterDifficulty", typeof(object), typeof(EncounterGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for FilterMinimumEnemies.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterMinimumEnemiesProperty =
        DependencyProperty.Register("FilterMinimumEnemies", typeof(double), typeof(EncounterGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for GridDataSource.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty GridDataSourceProperty =
        DependencyProperty.Register("GridDataSource", typeof(IEnumerable), typeof(EncounterGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for SearchTextChangeCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SearchTextChangeCommandProperty =
        DependencyProperty.Register("SearchTextChangeCommand", typeof(ICommand), typeof(EncounterGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for SelectEncounterCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SelectEncounterCommandProperty =
        DependencyProperty.Register("SelectEncounterCommand", typeof(ICommand), typeof(EncounterGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for SortCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SortCommandProperty =
        DependencyProperty.Register("SortCommand", typeof(ICommand), typeof(EncounterGrid), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for Suggestions.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SuggestionsProperty =
        DependencyProperty.Register("Suggestions", typeof(object), typeof(EncounterGrid), new PropertyMetadata(null));

    public EncounterGrid()
    {
        this.InitializeComponent();
    }

    public ICommand AddNewEncounterCommand
    {
        get => (ICommand)GetValue(AddNewEncounterCommandProperty);
        set => SetValue(AddNewEncounterCommandProperty, value);
    }

    public ICommand ClearFiltersCommand
    {
        get => (ICommand)GetValue(ClearFiltersCommandProperty);
        set => SetValue(ClearFiltersCommandProperty, value);
    }

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

    public object FilterCampaignSelected
    {
        get => (object)GetValue(FilterCampaignSelectedProperty);
        set => SetValue(FilterCampaignSelectedProperty, value);
    }

    public object FilterCampaignSource
    {
        get => (object)GetValue(FilterCampaignSourceProperty);
        set => SetValue(FilterCampaignSourceProperty, value);
    }

    public ICommand FilterCommand
    {
        get => (ICommand)GetValue(FilterCommandProperty);
        set => SetValue(FilterCommandProperty, value);
    }

    public object FilterEncounterDifficultySource
    {
        get => (object)GetValue(FilterEncounterDifficultySourceProperty);
        set => SetValue(FilterEncounterDifficultySourceProperty, value);
    }

    public object FilterMaximumEncounterDifficulty
    {
        get => (object)GetValue(FilterMaximumEncounterDifficultyProperty);
        set => SetValue(FilterMaximumEncounterDifficultyProperty, value);
    }

    public double FilterMaximumEnemies
    {
        get => (double)GetValue(FilterMaximumEnemiesProperty);
        set => SetValue(FilterMaximumEnemiesProperty, value);
    }

    public object FilterMinimumEncounterDifficulty
    {
        get => (object)GetValue(FilterMinimumEncounterDifficultyProperty);
        set => SetValue(FilterMinimumEncounterDifficultyProperty, value);
    }

    public double FilterMinimumEnemies
    {
        get => (double)GetValue(FilterMinimumEnemiesProperty);
        set => SetValue(FilterMinimumEnemiesProperty, value);
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

    public object Suggestions
    {
        get => (object)GetValue(SuggestionsProperty);
        set => SetValue(SuggestionsProperty, value);
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


    public bool InteractableRows
    {
        get => (bool)GetValue(InteractableRowsProperty);
        set => SetValue(InteractableRowsProperty, value);
    }

    // Using a DependencyProperty as the backing store for InteractableRows.  This enables animation, styling, binding, etc...s
    public static readonly DependencyProperty InteractableRowsProperty =
        DependencyProperty.Register("InteractableRows", typeof(bool), typeof(EncounterGrid), new PropertyMetadata(false));



    public bool HasVisibleFilters
    {
        get => (bool)GetValue(HasVisibleFiltersProperty);
        set => SetValue(HasVisibleFiltersProperty, value);
    }

    // Using a DependencyProperty as the backing store for HasVisibleFilters.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty HasVisibleFiltersProperty =
        DependencyProperty.Register("HasVisibleFilters", typeof(bool), typeof(EncounterGrid), new PropertyMetadata(false));


}