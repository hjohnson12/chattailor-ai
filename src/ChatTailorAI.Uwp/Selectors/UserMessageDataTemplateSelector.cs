using ChatTailorAI.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace ChatTailorAI.Uwp.Selectors
{
    public class UserMessageDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate UserMessageWithImageTemplate { get; set; }
        public DataTemplate UserMessageWithoutImageTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is ChatMessageViewModel)
            {
                var chatMessage = item as ChatMessageViewModel;

                if (chatMessage.IsImageMessage)
                {
                    return UserMessageWithImageTemplate;
                }
                else
                {
                    return UserMessageWithoutImageTemplate;
                }
            }

            return base.SelectTemplateCore(item, container);
        }
    }
}
