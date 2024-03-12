using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Windows.Input;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.ViewModels;
using System.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EasyEncounters.Views.UserControls;

public sealed partial class ConditionToggleButton : UserControl
{


    public IEnumerable DataSource
    {
        get
        {
            return (IEnumerable)GetValue(DataSourceProperty);
        }
        set
        {
            SetValue(DataSourceProperty, value);
        }
    }

    // Using a DependencyProperty as the backing store for DataSource.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DataSourceProperty =
        DependencyProperty.Register("DataSource", typeof(IEnumerable), typeof(ConditionToggleButton), new PropertyMetadata(null));




    public ICommand ToggleCommand
    {
        get
        {
            return (ICommand)GetValue(ToggleCommandProperty);
        }
        set
        {
            SetValue(ToggleCommandProperty, value);
        }
    }

    // Using a DependencyProperty as the backing store for ToggleCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ToggleCommandProperty =
        DependencyProperty.Register("ToggleCommand", typeof(ICommand), typeof(ConditionToggleButton), new PropertyMetadata(null));





    public ConditionsHelper ConditionsHelper
    {
        get
        {
            return (ConditionsHelper)GetValue(ConditionsHelperProperty);
        }
        set
        {
            SetValue(ConditionsHelperProperty, value);
        }
    }

    // Using a DependencyProperty as the backing store for ConditionsHelper.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ConditionsHelperProperty =
        DependencyProperty.Register("ConditionsHelper", typeof(ConditionsHelper), typeof(ConditionToggleButton), new PropertyMetadata(null));




    public Condition Condition
    {
        get
        {
            return (Condition)GetValue(ConditionProperty);
        }
        set
        {
            SetValue(ConditionProperty, value);
        }
    }

    // Using a DependencyProperty as the backing store for Condition.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ConditionProperty =
        DependencyProperty.Register("Condition", typeof(Condition), typeof(ConditionToggleButton), new PropertyMetadata(null));





    public ConditionToggleButton()
    {
        InitializeComponent();
    }
}
