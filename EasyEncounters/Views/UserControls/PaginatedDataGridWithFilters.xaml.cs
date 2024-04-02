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

namespace EasyEncounters.Views.UserControls;

public sealed partial class PaginatedDataGridWithFilters : UserControl
{
    // Using a DependencyProperty as the backing store for AddNewItemCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty AddNewItemCommandProperty =
        DependencyProperty.Register("AddNewItemCommand", typeof(ICommand), typeof(PaginatedDataGridWithFilters), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for DataGridControl.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DataGridControlProperty =
        DependencyProperty.Register("DataGridControl", typeof(object), typeof(PaginatedDataGridWithFilters), new PropertyMetadata(new Grid()));

    // Using a DependencyProperty as the backing store for FilterControl.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FiltersControlProperty =
        DependencyProperty.Register("FiltersControl", typeof(object), typeof(PaginatedDataGridWithFilters), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for GridFilteredValues.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty GridFilteredValuesProperty =
        DependencyProperty.Register("GridFilteredValues", typeof(GridFilteredValues), typeof(PaginatedDataGridWithFilters), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for TitleText.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TitleTextProperty =
        DependencyProperty.Register("TitleText", typeof(string), typeof(PaginatedDataGridWithFilters), new PropertyMetadata(""));

    public PaginatedDataGridWithFilters()
    {
        this.InitializeComponent();
    }

    public ICommand AddNewItemCommand
    {
        get => (ICommand)GetValue(AddNewItemCommandProperty);
        set => SetValue(AddNewItemCommandProperty, value);
    }

    public object DataGridControl
    {
        get => (object)GetValue(DataGridControlProperty);
        set => SetValue(DataGridControlProperty, value);
    }

    public object FiltersControl
    {
        get => (object)GetValue(FiltersControlProperty);
        set => SetValue(FiltersControlProperty, value);
    }

    public GridFilteredValues GridFilteredValues
    {
        get => (GridFilteredValues)GetValue(GridFilteredValuesProperty);
        set => SetValue(GridFilteredValuesProperty, value);
    }

    public string TitleText
    {
        get => (string)GetValue(TitleTextProperty);
        set => SetValue(TitleTextProperty, value);
    }
}