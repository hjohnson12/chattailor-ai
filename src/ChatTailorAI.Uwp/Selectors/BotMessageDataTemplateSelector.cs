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
    public class BotMessageDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BotMessageWithImageTemplate { get; set; }
        public DataTemplate BotMessageWithoutImageTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is ChatMessageViewModel)
            {
                var chatMessage = item as ChatMessageViewModel;

                if (chatMessage.IsImageMessage)
                {
                    return BotMessageWithImageTemplate;
                }
                else
                {
                    return BotMessageWithoutImageTemplate;
                }
            }

            return base.SelectTemplateCore(item, container);
        }
    }
}