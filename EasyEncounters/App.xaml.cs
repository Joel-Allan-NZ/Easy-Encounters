﻿using EasyEncounters.Activation;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Services;
using EasyEncounters.Core.Services.Models;
using EasyEncounters.Services;
using EasyEncounters.Services.Filter;
using EasyEncounters.ViewModels;
using EasyEncounters.ViewModels.EncounterTabs;
using EasyEncounters.Views;
using EasyEncounters.Views.EncounterEdit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

namespace EasyEncounters;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers

            // Services
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IModelOptionsService, ModelOptionsService>();
            services.AddSingleton<ITabService, TabService>();
            services.AddSingleton<ICreatureService, CreatureService>();
            services.AddSingleton<IAbilityService, AbilityService>();
            services.AddSingleton<IFilteringService, FilteringService>();

            // Core Services
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<IDataService, DataService>();
            services.AddSingleton<IRandomService, RandomService>();
            services.AddSingleton<IEncounterService, EncounterService>();
            services.AddSingleton<IActiveEncounterService, ActiveEncounterService>();
            services.AddSingleton<IPartyXPService, PartyXPService>();
            services.AddSingleton<IDiceService, DiceService>();
            services.AddSingleton<ILogService, LogService>();

            // Views and ViewModels
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<MainPage>();
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();
            services.AddTransient<CampaignSplashViewModel>();
            services.AddTransient<CampaignSplashPage>();
            services.AddTransient<PartySelectViewModel>();
            services.AddTransient<PartySelectPage>();
            services.AddTransient<EncounterSelectViewModel>();
            services.AddTransient<EncounterSelectPage>();
            services.AddTransient<CreatureCRUDViewModel>();
            services.AddTransient<CreatureCRUDPage>();
            services.AddTransient<CampaignCRUDViewModel>();
            services.AddTransient<CampaignCRUDPage>();
            services.AddTransient<PartyEditViewModel>();
            services.AddTransient<PartyEditPage>();
            services.AddTransient<PartyCRUDViewModel>();
            services.AddTransient<PartyCRUDPage>();
            services.AddTransient<EncounterCRUDViewModel>();
            services.AddTransient<EncounterCRUDPage>();
            services.AddTransient<EncounterTabPage>();
            services.AddTransient<EncounterTabViewModel>();
            services.AddTransient<AbilityEditPage>();
            services.AddTransient<AbilityEditViewModel>();
            services.AddTransient<AbilityCRUDPage>();
            services.AddTransient<AbilityCRUDViewModel>();
            services.AddTransient<EncounterDamageTabPage>();
            services.AddTransient<EncounterDamageTabViewModel>();
            services.AddTransient<CreatureDisplayTabPage>();
            services.AddTransient<CreatureDisplayTabViewModel>();
            services.AddTransient<LogTabPage>();
            services.AddTransient<LogTabViewModel>();
            services.AddTransient<CreatureEditNavigationPageViewModel>();
            services.AddTransient<CreatureEditNavigationPage>();
            services.AddTransient<EncounterEditNavigationViewModel>();
            services.AddTransient<EncounterEditNavigationPage>();
            services.AddTransient<EncounterAddCreaturesTabPage>();
            services.AddTransient<EncounterAddCreaturesTabViewModel>();



            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        }).
        Build();

        UnhandledException += App_UnhandledException;
    }

    public static UIElement? AppTitlebar
    {
        get; set;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        await App.GetService<IActivationService>().ActivateAsync(args);
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        var a = App.GetService<ILogService>();
        var exception = e.Exception;
        a.LogError(exception.Source + exception.Message + exception.StackTrace + exception.InnerException?.Message);
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
        //if (!hasHandledUnhandledException)
        //{
        //    e.Handled = true;
        //    var exception = e.Exception;
        //    _logger.Error("Unhandled exception - {0}", exception);
        //    await CrashHandler.ReportExceptionAsync(exception.Message, exception.StackTrace);
        //    hasHandledUnhandledException = true;
        //    throw e.Exception;
        //}
    }
}