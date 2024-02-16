using System.Reflection;
using EasyEncounters.Contracts.Services;
using EasyEncounters.ViewModels;

using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;

namespace EasyEncounters.Activation;

public class DefaultActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
{
    private readonly INavigationService _navigationService;

    public DefaultActivationHandler(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
    {
        // None of the ActivationHandlers has handled the activation.
        return _navigationService.Frame?.Content == null;
    }

    protected async override Task HandleInternalAsync(LaunchActivatedEventArgs args)
    {
        if (await RequireRedirect())
        {

            _navigationService.NavigateTo(typeof(MainViewModel).FullName!, args.Arguments);

            await Task.CompletedTask;
        }
    }

    private async Task<bool> RequireRedirect()
    {
        AppActivationArguments args = AppInstance.GetCurrent().GetActivatedEventArgs();
        var isRedirect = false;
        //get version info to use as appinstance registery key
        Version version = Assembly.GetExecutingAssembly().GetName().Version!;
        //Discover if already running
        AppInstance keyInstance = AppInstance.FindOrRegisterForKey($"EasyEncounters{version.Major}.{version.Minor}.{version.Build}.{version.Revision}");

        if (keyInstance.IsCurrent)
        {
            //TODO: create a reactivation event to handle being reactivated by a redirect.
            keyInstance.Activated += OnActivated;
        }
        else
        {
            isRedirect = true;

            await keyInstance.RedirectActivationToAsync(args);

            App.Current.Exit();
        }


        return isRedirect;
    }

    private void OnActivated(object? sender, AppActivationArguments e)
    {
        App.MainWindow.BringToFront();
    }

}
