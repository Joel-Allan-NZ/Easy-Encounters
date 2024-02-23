using EasyEncounters.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace EasyEncounters.Views;

// To learn more about WebView2, see https://docs.microsoft.com/microsoft-edge/webview2/.
public sealed partial class WebViewPage : Page
{
    public WebViewPage()
    {
        ViewModel = App.GetService<WebViewViewModel>();
        InitializeComponent();

        ViewModel.WebViewService.Initialize(WebView);
    }

    public WebViewViewModel ViewModel
    {
        get;
    }
}