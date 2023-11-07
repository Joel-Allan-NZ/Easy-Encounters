using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Permissions;
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

namespace EasyEncounters.Views;
public sealed partial class SpellGrid : UserControl
{
    public SpellGrid()
    {
        this.InitializeComponent();
    }

    public ICommand SortCommand
    {
        get => (ICommand)GetValue(SortCommandProperty);
        set => SetValue(SortCommandProperty, value);
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SortCommandProperty =
        DependencyProperty.Register("SortCommand", typeof(ICommand), typeof(SpellGrid), new PropertyMetadata(null));

    public ICommand FilterCommand
    {
        get => (ICommand)GetValue(FilterCommandProperty);
        set => SetValue(FilterCommandProperty, value);
    }

    // Using a DependencyProperty as the backing store for FilterCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterCommandProperty =
        DependencyProperty.Register("FilterCommand", typeof(ICommand), typeof(SpellGrid), new PropertyMetadata(null));

    public ICommand SearchTextChangeCommand
    {
        get => (ICommand)GetValue(SearchTextChangeCommandProperty);
        set => SetValue(SearchTextChangeCommandProperty, value);
    }

    // Using a DependencyProperty as the backing store for SearchTextChangeCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SearchTextChangeCommandProperty =
        DependencyProperty.Register("SearchTextChangeCommand", typeof(ICommand), typeof(SpellGrid), new PropertyMetadata(null));

    public IEnumerable GridDataSource
    {
        get => (IEnumerable)GetValue(GridDataSourceProperty);
        set => SetValue(GridDataSourceProperty, value);
    }

    // Using a DependencyProperty as the backing store for GridDataSource.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty GridDataSourceProperty =
        DependencyProperty.Register("GridDataSource", typeof(IEnumerable), typeof(SpellGrid), new PropertyMetadata(null));

    public ICommand EditSpellCommand
    {
        get => (ICommand)GetValue(EditSpellCommandProperty);
        set => SetValue(EditSpellCommandProperty, value);
    }

    // Using a DependencyProperty as the backing store for EditSpellCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty EditSpellCommandProperty =
        DependencyProperty.Register("EditSpellCommand", typeof(ICommand), typeof(SpellGrid), new PropertyMetadata(null));

    public ICommand AddSpellCommand
    {
        get => (ICommand)GetValue(AddSpellCommandProperty);
        set => SetValue(AddSpellCommandProperty, value);
    }

    // Using a DependencyProperty as the backing store for AddSpellCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty AddSpellCommandProperty =
        DependencyProperty.Register("AddSpellCommand", typeof(ICommand), typeof(SpellGrid), new PropertyMetadata(null));

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

    public object Suggestions
    {
        get => (object)GetValue(SuggestionsProperty);
        set => SetValue(SuggestionsProperty, value);
    }

    // Using a DependencyProperty as the backing store for Suggestions.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SuggestionsProperty =
        DependencyProperty.Register("Suggestions", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));



    public object FilterMinimumSpellLevel
    {
        get => (object)GetValue(FilterMinimumSpellLevelProperty);
        set => SetValue(FilterMinimumSpellLevelProperty, value);
    }

    // Using a DependencyProperty as the backing store for FilterMinimumSpellLevel.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterMinimumSpellLevelProperty =
        DependencyProperty.Register("FilterMinimumSpellLevel", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));



    public object FilterMaximumSpellLevel
    {
        get => (object)GetValue(FilterMaximumSpellLevelProperty);
        set => SetValue(FilterMaximumSpellLevelProperty, value);
    }

    // Using a DependencyProperty as the backing store for FilterMaximumSpellLevel.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterMaximumSpellLevelProperty =
        DependencyProperty.Register("FilterMaximumSpellLevel", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));



    public object FilterSpellLevelSource
    {
        get => (object)GetValue(FilterSpellLevelSourceProperty);
        set => SetValue(FilterSpellLevelSourceProperty, value);
    }

    // Using a DependencyProperty as the backing store for FilterSpellLevelSource.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterSpellLevelSourceProperty =
        DependencyProperty.Register("FilterSpellLevelSource", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));



    public object FilterSpellSchoolSource
    {
        get => (object)GetValue(FilterSpellSchoolSourceProperty);
        set => SetValue(FilterSpellSchoolSourceProperty, value);
    }

    // Using a DependencyProperty as the backing store for FilterSpellSchoolSource.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterSpellSchoolSourceProperty =
        DependencyProperty.Register("FilterSpellSchoolSource", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));



    public object FilterSpellSchoolSelected
    {
        get => (object)GetValue(FilterSpellSchoolSelectedProperty);
        set => SetValue(FilterSpellSchoolSelectedProperty, value);
    }

    // Using a DependencyProperty as the backing store for FilterSpellSchoolSelected.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterSpellSchoolSelectedProperty =
        DependencyProperty.Register("FilterSpellSchoolSelected", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));



    public object FilterConcentrationSource
    {
        get => GetValue(FilterConcentrationSourceProperty);
        set => SetValue(FilterConcentrationSourceProperty, value);
    }

    // Using a DependencyProperty as the backing store for FilterConcentrationSource.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterConcentrationSourceProperty =
        DependencyProperty.Register("FilterConcentrationSource", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));


    public object FilterConcentrationSelected
    {
        get
        {
            return GetValue(FilterConcentrationSelectedProperty);
        }
        set => SetValue(FilterConcentrationSelectedProperty, value);
    }

    // Using a DependencyProperty as the backing store for FilterConcentrationSelected.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterConcentrationSelectedProperty =
        DependencyProperty.Register("FilterConcentrationSelected", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));


    public object FilterDamageTypeSource
    {
        get
        {
            return GetValue(FilterDamageTypeSourceProperty);
        }
        set => SetValue(FilterDamageTypeSourceProperty, value);
    }

    // Using a DependencyProperty as the backing store for FilterDamageTypeSource.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterDamageTypeSourceProperty =
        DependencyProperty.Register("FilterDamageTypeSource", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));


    public object FilterDamageTypeSelected
    {
        get => (object)GetValue(FilterDamageTypeSelectedProperty);
        set => SetValue(FilterDamageTypeSelectedProperty, value);
    }

    // Using a DependencyProperty as the backing store for FilterDamageTypeSelected.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterDamageTypeSelectedProperty =
        DependencyProperty.Register("FilterDamageTypeSelected", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));


    public object FilterResolutionSource
    {
        get
        {
            var filterResolutionSourceProperty = FilterResolutionSourceProperty;
            return (object)GetValue(filterResolutionSourceProperty);
        }
        set => SetValue(FilterResolutionSourceProperty, value);
    }

    // Using a DependencyProperty as the backing store for FilterResolutionSource.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterResolutionSourceProperty =
        DependencyProperty.Register("FilterResolutionSource", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));


    public object FilterResolutionSelected
    {
        get => (object)GetValue(FilterResolutionSelectedProperty);
        set => SetValue(FilterResolutionSelectedProperty, value);
    }

    // Using a DependencyProperty as the backing store for FilterResolutionSelected.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterResolutionSelectedProperty =
        DependencyProperty.Register("FilterResolutionSelected", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));






}
