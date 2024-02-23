using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;

namespace EasyEncounters.Contracts.Services;

public interface IWebViewService
{
    event EventHandler<CoreWebView2WebErrorStatus>? NavigationCompleted;

    bool CanGoBack
    {
        get;
    }

    bool CanGoForward
    {
        get;
    }

    Uri? Source
    {
        get;
    }

    void GoBack();

    void GoForward();

    void Initialize(WebView2 webView);

    void Reload();

    void UnregisterEvents();
}