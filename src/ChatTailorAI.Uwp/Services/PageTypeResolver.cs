using ChatTailorAI.Shared.Enums;
using ChatTailorAI.Shared.Services.Common.Navigation;
using ChatTailorAI.Uwp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Uwp.Services
{
    public class PageTypeResolver : IPageTypeResolver
    {
        public Type GetPageType(NavigationPageKeys pageKey)
        {
            switch (pageKey)
            {
                case NavigationPageKeys.ChatPage:
                    return typeof(ChatPage);
                case NavigationPageKeys.SettingsPage:
                    return typeof(SettingsPage);
                case NavigationPageKeys.PromptsPage:
                    return typeof(PromptsPage);
                case NavigationPageKeys.AssistantsPage:
                    return typeof(AssistantsPage);
                case NavigationPageKeys.ChatsPage:
                    return typeof(ChatsPage);
                case NavigationPageKeys.ImagesPage:
                    return typeof(ImagesPage);
                case NavigationPageKeys.ShellPage:
                    return typeof(ShellPage);
                default:
                    throw new NotImplementedException("Page type not implemented for the given key.");
            }
        }
    }
}