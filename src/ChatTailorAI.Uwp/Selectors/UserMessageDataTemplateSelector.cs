using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using ChatTailorAI.Shared.ViewModels;

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
