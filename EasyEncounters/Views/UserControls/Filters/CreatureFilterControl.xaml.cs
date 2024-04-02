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
public sealed partial class CreatureFilterControl : UserControl
{
    public CreatureFilterControl()
    {
        this.InitializeComponent();
    }

    //// Using a DependencyProperty as the backing store for FilterAlignmentSelected.  This enables animation, styling, binding, etc...
    //public static readonly DependencyProperty FilterAlignmentSelectedProperty =
    //    DependencyProperty.Register("FilterAlignmentSelected", typeof(object), typeof(CreatureGrid), new PropertyMetadata(null));

    //// Using a DependencyProperty as the backing store for FilterAlignmentSource.  This enables animation, styling, binding, etc...
    //public static readonly DependencyProperty FilterAlignmentSourceProperty =
    //    DependencyProperty.Register("FilterAlignmentSource", typeof(object), typeof(CreatureGrid), new PropertyMetadata(null));

    //// Using a DependencyProperty as the backing store for FilterCreatureTypeSelected.  This enables animation, styling, binding, etc...
    //public static readonly DependencyProperty FilterCreatureTypeSelectedProperty =
    //    DependencyProperty.Register("FilterCreatureTypeSelected", typeof(object), typeof(CreatureGrid), new PropertyMetadata(null));

    //// Using a DependencyProperty as the backing store for FilterCreatureTypeSource.  This enables animation, styling, binding, etc...
    //public static readonly DependencyProperty FilterCreatureTypeSourceProperty =
    //    DependencyProperty.Register("FilterCreatureTypeSource", typeof(object), typeof(CreatureGrid), new PropertyMetadata(null));

    //// Using a DependencyProperty as the backing store for FilterMaximumCR.  This enables animation, styling, binding, etc...
    //public static readonly DependencyProperty FilterMaximumCRProperty =
    //    DependencyProperty.Register("FilterMaximumCR", typeof(double), typeof(CreatureGrid), new PropertyMetadata(null));

    //// Using a DependencyProperty as the backing store for FilterMaximumSize.  This enables animation, styling, binding, etc...
    //public static readonly DependencyProperty FilterMaximumSizeProperty =
    //    DependencyProperty.Register("FilterMaximumSize", typeof(object), typeof(CreatureGrid), new PropertyMetadata(null));

    //// Using a DependencyProperty as the backing store for FilterMinimumCR.  This enables animation, styling, binding, etc...
    //public static readonly DependencyProperty FilterMinimumCRProperty =
    //    DependencyProperty.Register("FilterMinimumCR", typeof(double), typeof(CreatureGrid), new PropertyMetadata(null));

    //// Using a DependencyProperty as the backing store for FilterMinimumSize.  This enables animation, styling, binding, etc...
    //public static readonly DependencyProperty FilterMinimumSizeProperty =
    //    DependencyProperty.Register("FilterMinimumSize", typeof(object), typeof(CreatureGrid), new PropertyMetadata(null));

    //// Using a DependencyProperty as the backing store for FilterSizeSource.  This enables animation, styling, binding, etc...
    //public static readonly DependencyProperty FilterSizeSourceProperty =
    //    DependencyProperty.Register("FilterSizeSource", typeof(object), typeof(CreatureGrid), new PropertyMetadata(null));

    //public object FilterAlignmentSelected
    //{
    //    get => (object)GetValue(FilterAlignmentSelectedProperty);
    //    set => SetValue(FilterAlignmentSelectedProperty, value);
    //}

    //public object FilterAlignmentSource
    //{
    //    get => (object)GetValue(FilterAlignmentSourceProperty);
    //    set => SetValue(FilterAlignmentSourceProperty, value);
    //}

    //public object FilterCreatureTypeSelected
    //{
    //    get => GetValue(FilterCreatureTypeSelectedProperty);
    //    set => SetValue(FilterCreatureTypeSelectedProperty, value);
    //}

    //public object FilterCreatureTypeSource
    //{
    //    get => (object)GetValue(FilterCreatureTypeSourceProperty);
    //    set => SetValue(FilterCreatureTypeSourceProperty, value);
    //}

    //public double FilterMaximumCR
    //{
    //    get => (double)GetValue(FilterMaximumCRProperty);
    //    set => SetValue(FilterMaximumCRProperty, value);
    //}

    //public object FilterMaximumSize
    //{
    //    get => (object)GetValue(FilterMaximumSizeProperty);
    //    set => SetValue(FilterMaximumSizeProperty, value);
    //}

    //public double FilterMinimumCR
    //{
    //    get => (double)GetValue(FilterMinimumCRProperty);
    //    set => SetValue(FilterMinimumCRProperty, value);
    //}

    //public object FilterMinimumSize
    //{
    //    get => (object)GetValue(FilterMinimumSizeProperty);
    //    set => SetValue(FilterMinimumSizeProperty, value);
    //}

    //public object FilterSizeSource
    //{
    //    get => (object)GetValue(FilterSizeSourceProperty);
    //    set => SetValue(FilterSizeSourceProperty, value);
    //}




    public CreatureFilter CreatureFilter
    {
        get => (CreatureFilter)GetValue(CreatureFilterProperty);
        set => SetValue(CreatureFilterProperty, value);
    }

    // Using a DependencyProperty as the backing store for CreatureFilter.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CreatureFilterProperty =
        DependencyProperty.Register("CreatureFilter", typeof(CreatureFilter), typeof(CreatureFilterControl), new PropertyMetadata(null));




}
