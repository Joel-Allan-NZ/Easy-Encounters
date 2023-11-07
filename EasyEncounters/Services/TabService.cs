using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Models;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Printing;

namespace EasyEncounters.Services
{
    public class TabService : ITabService
    {
        private const string _viewModelString = "ViewModel";
        private readonly IPageService _pageService;
        public TabService(IPageService pageService)
        {
            _pageService = pageService;
        }

        public ObservableRecipientTab OpenTab(string tabKey, object parameter, string? name = null, bool closeable = true) //todo: add closeable boolean, add logtab that isn't closeable.
        {
            //var concreteType = tab.GetType();

            var pageType = _pageService.GetPageType(tabKey);
            //need to do vaguely reflecty bullshit to get ITabAware viewmodel stuff. See: NavigateTo "_frame.GetPageVieWModel()" extension
            //tab.Content = (Page)Activator.CreateInstance(pageType); //should be safe, but? todo: safety
            var page = (Page?)Activator.CreateInstance(pageType);

            var pageVM = GetPageViewModel(page);

            if (pageVM is ObservableRecipientTab)
            {
                var obRecipTab = (ObservableRecipientTab)pageVM;
                obRecipTab.Content = page;
                obRecipTab.TabName = name ?? "New Tab";
                obRecipTab.IsClosable = closeable;
                obRecipTab.OnTabOpened(parameter);
                return obRecipTab;
            }
            throw new ArgumentException("Tab Key does not refer to a valid Observable Recipient Tab");
        }

        private object? GetPageViewModel(Page? page) => page?.GetType().GetProperty("ViewModel")?.GetValue(page, null);

        public void CloseTab(ObservableRecipientTab tab)
        {
            tab.OnTabClosed();
        }

        //public ITab OpenTab(string tabName, string pageKey, object parameter)
        //{
        //    var pageType = _pageService.GetPageType(pageKey);

        //    if (pageType != null)
        //    {
        //        var page = Activator.CreateInstance(pageType);
        //        if(page is Page )
        //    }
        //}

        //public ITab GenerateTab(string name, string pageKey, object parameter)
        //{
        //    //very hacky. TODO: improve.

        //    var pageType = _pageService.GetPageType(pageKey);

        //    if (pageType != null)
        //    {
        //        var page = Activator.CreateInstance(pageType);
        //        if(page is Page)
        //        {
        //            var vm = pageType.GetProperty(_viewModelString);
        //            if(vm != null)
        //            {
        //                var viewModel = vm.GetValue(page);
        //                if (viewModel != null && viewModel is INavigationAware)
        //                {
        //                    ((INavigationAware)viewModel).OnNavigatedTo(parameter);
        //                    return new Tab(name, (Page)page);
        //                }
        //            }
        //        }
        //    }
        //    throw new Exception("pageKey is not INavigationAware");
        //}
    }
}
