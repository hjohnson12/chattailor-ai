using System;
using Windows.UI.Xaml.Controls;
using ChatTailorAI.Shared.Enums;
using ChatTailorAI.Shared.Services.Common.Navigation;

namespace ChatTailorAI.Services.Uwp.UI.Navigation
{
    public class ChatPageNavigationService : IChatPageNavigationService
    {
        private readonly IPageTypeResolver _pageTypeResolver;
        public object ChatFrame { get; set; }

        public ChatPageNavigationService(IPageTypeResolver pageTypeResolver) 
        {
            _pageTypeResolver = pageTypeResolver;
        }

        public void NavigateTo(Type type)
        {
            Navigate(type);
        }

        public void NavigateTo(Type type, object parameter)
        {
            Navigate(type, parameter);
        }

        public void NavigateTo(NavigationPageKeys pageKey, object parameter)
        {
            var pageType = _pageTypeResolver.GetPageType(pageKey);
            Navigate(pageType, parameter);
        }

        private void Navigate(Type pageType, object parameter = null)
        {
            if (ChatFrame is Frame frame)
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
    }
}