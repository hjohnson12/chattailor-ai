using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using ChatTailorAI.Shared.Models.Chat.OpenAI;
using ChatTailorAI.Shared.ViewModels;

namespace ChatTailorAI.Uwp.Selectors
{
    public class ChatMessageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate UserMessageTemplate { get; set; }
        public DataTemplate BotMessageTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var message = (ChatMessageViewModel)item;

            if (message.IsUser)
            {
                return UserMessageTemplate;
            }
            else
            {
                return BotMessageTemplate;
            }
        }
    }
}
