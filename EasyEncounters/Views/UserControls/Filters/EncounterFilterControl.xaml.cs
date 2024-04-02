using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using EasyEncounters.Services.Filter;
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

namespace EasyEncounters.Views.UserControls.Filters;
public sealed partial class EncounterFilterControl : UserControl
{
    public EncounterFilterControl()
    {
        this.InitializeComponent();
    }
    //public object FilterCampaignSelected
    //{
    //    get => (object)GetValue(FilterCampaignSelectedProperty);
    //    set => SetValue(FilterCampaignSelectedProperty, value);
    //}

    public object FilterCampaignSource
    {
        get => (object)GetValue(FilterCampaignSourceProperty);
        set => SetValue(FilterCampaignSourceProperty, value);
    }

    //public object FilterEncounterDifficultySource
    //{
    //    get => (object)GetValue(FilterEncounterDifficultySourceProperty);
    //    set => SetValue(FilterEncounterDifficultySourceProperty, value);
    //}

    //public object FilterMaximumEncounterDifficulty
    //{
    //    get => (object)GetValue(FilterMaximumEncounterDifficultyProperty);
    //    set => SetValue(FilterMaximumEncounterDifficultyProperty, value);
    //}

    //public double FilterMaximumEnemies
    //{
    //    get => (double)GetValue(FilterMaximumEnemiesProperty);
    //    set => SetValue(FilterMaximumEnemiesProperty, value);
    //}

    //public object FilterMinimumEncounterDifficulty
    //{
    //    get => (object)GetValue(FilterMinimumEncounterDifficultyProperty);
    //    set => SetValue(FilterMinimumEncounterDifficultyProperty, value);
    //}

    //public double FilterMinimumEnemies
    //{
    //    get => (double)GetValue(FilterMinimumEnemiesProperty);
    //    set => SetValue(FilterMinimumEnemiesProperty, value);
    //}

//// Using a DependencyProperty as the backing store for FilterCampaignSelected.  This enables animation, styling, binding, etc...
//public static readonly DependencyProperty FilterCampaignSelectedProperty =
//    DependencyProperty.Register("FilterCampaignSelected", typeof(object), typeof(EncounterGrid), new PropertyMetadata(null));

//Using a DependencyProperty as the backing store for FilterCampaignSource.This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterCampaignSourceProperty =
        DependencyProperty.Register("FilterCampaignSource", typeof(object), typeof(EncounterGrid), new PropertyMetadata(null));

//// Using a DependencyProperty as the backing store for FilterEncounterDifficultySource.  This enables animation, styling, binding, etc...
//public static readonly DependencyProperty FilterEncounterDifficultySourceProperty =
//    DependencyProperty.Register("FilterEncounterDifficultySource", typeof(object), typeof(EncounterGrid), new PropertyMetadata(null));

//// Using a DependencyProperty as the backing store for FilterMaximumEncounterDifficulty.  This enables animation, styling, binding, etc...
//public static readonly DependencyProperty FilterMaximumEncounterDifficultyProperty =
//    DependencyProperty.Register("FilterMaximumEncounterDifficulty", typeof(object), typeof(EncounterGrid), new PropertyMetadata(null));

//// Using a DependencyProperty as the backing store for FilterMaximumEnemies.  This enables animation, styling, binding, etc...
//public static readonly DependencyProperty FilterMaximumEnemiesProperty =
//    DependencyProperty.Register("FilterMaximumEnemies", typeof(double), typeof(EncounterGrid), new PropertyMetadata(null));

//// Using a DependencyProperty as the backing store for FilterMinimumEncounterDifficulty.  This enables animation, styling, binding, etc...
//public static readonly DependencyProperty FilterMinimumEncounterDifficultyProperty =
//    DependencyProperty.Register("FilterMinimumEncounterDifficulty", typeof(object), typeof(EncounterGrid), new PropertyMetadata(null));

//// Using a DependencyProperty as the backing store for FilterMinimumEnemies.  This enables animation, styling, binding, etc...
//public static readonly DependencyProperty FilterMinimumEnemiesProperty =
//    DependencyProperty.Register("FilterMinimumEnemies", typeof(double), typeof(EncounterGrid), new PropertyMetadata(null));




public EncounterFilter EncounterFilter
    {
        get => (EncounterFilter)GetValue(EncounterFilterProperty);
        set => SetValue(EncounterFilterProperty, value);
    }

    // Using a DependencyProperty as the backing store for EncounterFilter.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty EncounterFilterProperty =
        DependencyProperty.Register("EncounterFilter", typeof(EncounterFilter), typeof(EncounterFilterControl), new PropertyMetadata(null));




}
