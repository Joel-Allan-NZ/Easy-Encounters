using EasyEncounters.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace EasyEncounters.Views;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }

    public MainViewModel ViewModel
    {
        get;
    }
}