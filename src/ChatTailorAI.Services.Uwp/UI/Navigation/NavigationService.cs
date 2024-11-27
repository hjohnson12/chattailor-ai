using System;
using Windows.UI.Xaml.Controls;
using ChatTailorAI.Shared.Enums;
using ChatTailorAI.Shared.Services.Common.Navigation;

namespace ChatTailorAI.Uwp.Services.UI.Navigation
{
    /// <summary>
    /// A class for handling the applications navigation within a frame.
    /// </summary>
    public class NavigationService : INavigationService
    {
        private readonly IPageTypeResolver _pageTypeResolver;
        public object MainFrame { get; set; }

        public NavigationService(IPageTypeResolver pageTypeResolver)
        {
            _pageTypeResolver = pageTypeResolver;
        }

        private void Navigate(Type pageType, object parameter = null)
        {
            if (MainFrame is Frame frame)
            {
                if (parameter == null)
                {
                    frame.Navigate(pageType);
                }
                else
                {
                    frame.Navigate(pageType, parameter);
                }
            }
        }

        public void NavigateTo(Type type)
        {
            Navigate(type);
        }

        public void NavigateTo(Type type, object parameter)
        {
            Navigate(type, parameter);
        }

        public void NavigateTo(NavigationPageKeys pageKey)
        {
            var pageType = _pageTypeResolver.GetPageType(pageKey);
            Navigate(pageType);
        }

        public void NavigateTo(NavigationPageKeys pageKey, object parameter)
        {
            var pageType = _pageTypeResolver.GetPageType(pageKey);
            Navigate(pageType, parameter);
        }

        public void NavigateToDefaultView()
        {
            var defaultPageType = _pageTypeResolver.GetPageType(NavigationPageKeys.ChatPage);
            Navigate(defaultPageType);
        }

        public void NavigateToSettings()
        {
            var settingsPageType = _pageTypeResolver.GetPageType(NavigationPageKeys.SettingsPage);
            Navigate(settingsPageType);
        }

        public void NavigateBack()
        {
            if (MainFrame is Frame frame)
            {
                frame.GoBack();
            }
        }
    }
}