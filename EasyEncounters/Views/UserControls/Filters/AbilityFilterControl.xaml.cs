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
public sealed partial class AbilityFilterControl : UserControl
{
    public AbilityFilterControl()
    {
        this.InitializeComponent();
    }

    //public object FilterActionSpeedSelected
    //{
    //    get => (object)GetValue(FilterActionSpeedSelectedProperty);
    //    set => SetValue(FilterActionSpeedSelectedProperty, value);
    //}

    //public object FilterActionSpeedSource
    //{
    //    get => (object)GetValue(FilterActionSpeedSourceProperty);
    //    set => SetValue(FilterActionSpeedSourceProperty, value);
    //}

    //public object FilterConcentrationSelected
    //{
    //    get => GetValue(FilterConcentrationSelectedProperty);
    //    set => SetValue(FilterConcentrationSelectedProperty, value);
    //}

    //public object FilterConcentrationSource
    //{
    //    get => GetValue(FilterConcentrationSourceProperty);
    //    set => SetValue(FilterConcentrationSourceProperty, value);
    //}

    //public object FilterDamageTypeSelected
    //{
    //    get => (object)GetValue(FilterDamageTypeSelectedProperty);
    //    set => SetValue(FilterDamageTypeSelectedProperty, value);
    //}

    //public object FilterDamageTypeSource
    //{
    //    get => GetValue(FilterDamageTypeSourceProperty);
    //    set => SetValue(FilterDamageTypeSourceProperty, value);
    //}

    //public object FilterMaximumSpellLevel
    //{
    //    get => (object)GetValue(FilterMaximumSpellLevelProperty);
    //    set => SetValue(FilterMaximumSpellLevelProperty, value);
    //}

    //public object FilterMinimumSpellLevel
    //{
    //    get => (object)GetValue(FilterMinimumSpellLevelProperty);
    //    set => SetValue(FilterMinimumSpellLevelProperty, value);
    //}

    //public object FilterResolutionSelected
    //{
    //    get => (object)GetValue(FilterResolutionSelectedProperty);
    //    set => SetValue(FilterResolutionSelectedProperty, value);
    //}

    //public object FilterResolutionSource
    //{
    //    get
    //    {
    //        var filterResolutionSourceProperty = FilterResolutionSourceProperty;
    //        return (object)GetValue(filterResolutionSourceProperty);
    //    }
    //    set => SetValue(FilterResolutionSourceProperty, value);
    //}

    //public object FilterSpellLevelSource
    //{
    //    get => (object)GetValue(FilterSpellLevelSourceProperty);
    //    set => SetValue(FilterSpellLevelSourceProperty, value);
    //}

    //public object FilterSpellSchoolSelected
    //{
    //    get => (object)GetValue(FilterSpellSchoolSelectedProperty);
    //    set => SetValue(FilterSpellSchoolSelectedProperty, value);
    //}

    //public object FilterSpellSchoolSource
    //{
    //    get => (object)GetValue(FilterSpellSchoolSourceProperty);
    //    set => SetValue(FilterSpellSchoolSourceProperty, value);
    //}

    //// Using a DependencyProperty as the backing store for FilterActionSpeedSelectedProperty.  This enables animation, styling, binding, etc...
    //public static readonly DependencyProperty FilterActionSpeedSelectedProperty =
    //    DependencyProperty.Register("FilterActionSpeedSelected", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));

    //// Using a DependencyProperty as the backing store for FilterActionSpeedSource.  This enables animation, styling, binding, etc...
    //public static readonly DependencyProperty FilterActionSpeedSourceProperty =
    //    DependencyProperty.Register("FilterActionSpeedSource", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));

    //// Using a DependencyProperty as the backing store for FilterConcentrationSelected.  This enables animation, styling, binding, etc...
    //public static readonly DependencyProperty FilterConcentrationSelectedProperty =
    //    DependencyProperty.Register("FilterConcentrationSelected", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));

    //// Using a DependencyProperty as the backing store for FilterConcentrationSource.  This enables animation, styling, binding, etc...
    //public static readonly DependencyProperty FilterConcentrationSourceProperty =
    //    DependencyProperty.Register("FilterConcentrationSource", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));

    //// Using a DependencyProperty as the backing store for FilterDamageTypeSelected.  This enables animation, styling, binding, etc...
    //public static readonly DependencyProperty FilterDamageTypeSelectedProperty =
    //    DependencyProperty.Register("FilterDamageTypeSelected", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));

    //// Using a DependencyProperty as the backing store for FilterDamageTypeSource.  This enables animation, styling, binding, etc...
    //public static readonly DependencyProperty FilterDamageTypeSourceProperty =
    //    DependencyProperty.Register("FilterDamageTypeSource", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));

    //// Using a DependencyProperty as the backing store for FilterMaximumSpellLevel.  This enables animation, styling, binding, etc...
    //public static readonly DependencyProperty FilterMaximumSpellLevelProperty =
    //    DependencyProperty.Register("FilterMaximumSpellLevel", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));

    //// Using a DependencyProperty as the backing store for FilterMinimumSpellLevel.  This enables animation, styling, binding, etc...
    //public static readonly DependencyProperty FilterMinimumSpellLevelProperty =
    //    DependencyProperty.Register("FilterMinimumSpellLevel", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));

    //// Using a DependencyProperty as the backing store for FilterResolutionSelected.  This enables animation, styling, binding, etc...
    //public static readonly DependencyProperty FilterResolutionSelectedProperty =
    //    DependencyProperty.Register("FilterResolutionSelected", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));

    //// Using a DependencyProperty as the backing store for FilterResolutionSource.  This enables animation, styling, binding, etc...
    //public static readonly DependencyProperty FilterResolutionSourceProperty =
    //    DependencyProperty.Register("FilterResolutionSource", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));

    //// Using a DependencyProperty as the backing store for FilterSpellLevelSource.  This enables animation, styling, binding, etc...
    //public static readonly DependencyProperty FilterSpellLevelSourceProperty =
    //    DependencyProperty.Register("FilterSpellLevelSource", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));

    //// Using a DependencyProperty as the backing store for FilterSpellSchoolSelected.  This enables animation, styling, binding, etc...
    //public static readonly DependencyProperty FilterSpellSchoolSelectedProperty =
    //    DependencyProperty.Register("FilterSpellSchoolSelected", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));

    //// Using a DependencyProperty as the backing store for FilterSpellSchoolSource.  This enables animation, styling, binding, etc...
    //public static readonly DependencyProperty FilterSpellSchoolSourceProperty =
    //    DependencyProperty.Register("FilterSpellSchoolSource", typeof(object), typeof(SpellGrid), new PropertyMetadata(null));




    public AbilityFilter AbilityFilter
    {
        get => (AbilityFilter)GetValue(AbilityFilterProperty);
        set => SetValue(AbilityFilterProperty, value);
    }

    // Using a DependencyProperty as the backing store for AbilityFilter.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty AbilityFilterProperty =
        DependencyProperty.Register("AbilityFilter", typeof(AbilityFilter), typeof(AbilityFilterControl), new PropertyMetadata(null));



}
