using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Microsoft.Xaml.Interactivity;

namespace ChatTailorAI.Uwp.Behaviors
{
    public class ListViewFlyoutBehavior : Behavior<ListView>
    {
        public static readonly DependencyProperty DeleteMessageCommandProperty =
                    DependencyProperty.Register(
                        "DeleteMessageCommand",
                        typeof(ICommand),
                        typeof(ListViewFlyoutBehavior),
                        new PropertyMetadata(null));

        public ICommand DeleteMessageCommand
        {
            get { return (ICommand)GetValue(DeleteMessageCommandProperty); }
            set { SetValue(DeleteMessageCommandProperty, value); }
        }

        public static readonly DependencyProperty CopyMessageCommandProperty =
                    DependencyProperty.Register(
                        "CopyMessageCommand",
                        typeof(ICommand),
                        typeof(ListViewFlyoutBehavior),
                        new PropertyMetadata(null));

        public ICommand CopyMessageCommand
        {
            get { return (ICommand)GetValue(CopyMessageCommandProperty); }
            set { SetValue(CopyMessageCommandProperty, value); }
        }

        public static readonly DependencyProperty CopyToPromptCommandProperty =
                   DependencyProperty.Register(
                       "CopyToPromptCommand",
                       typeof(ICommand),
                       typeof(ListViewFlyoutBehavior),
                       new PropertyMetadata(null));

        public ICommand CopyToPromptCommand
        {
            get { return (ICommand)GetValue(CopyToPromptCommandProperty); }
            set { SetValue(CopyToPromptCommandProperty, value); }
        }

        protected override void OnAttached()
        {
            AssociatedObject.ItemClick += AssociatedObject_ItemClick;
            AssociatedObject.RightTapped += AssociatedObject_RightTapped;
        }

        private void AssociatedObject_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            var listView = sender as ListView;
            var frameworkElement = e.OriginalSource as FrameworkElement;

            if (frameworkElement != null)
            {
                // Items data, in our case ChatCompletionMessage
                var data = frameworkElement.DataContext;

                var listViewItem = listView.ContainerFromItem(data) as ListViewItem;

                if (listViewItem != null)
                {
                    var flyout = FlyoutBase.GetAttachedFlyout(listViewItem);
                    if (flyout is MenuFlyout menuFlyout)
                    {
                        var deleteItem = menuFlyout.Items[0] as MenuFlyoutItem;
                        if (deleteItem != null)
                        {
                            deleteItem.Command = this.DeleteMessageCommand;
                            deleteItem.CommandParameter = data;
                        }

                        var copyItem = menuFlyout.Items[1] as MenuFlyoutItem;
                        if (copyItem != null)
                        {
                            copyItem.Command = this.CopyMessageCommand;
                            copyItem.CommandParameter = data;
                        }

                        var copyToPromptItem = menuFlyout.Items[2] as MenuFlyoutItem;
                        if (copyToPromptItem != null)
                        {
                            copyToPromptItem.Command = this.CopyToPromptCommand;
                            copyToPromptItem.CommandParameter = data;
                        }
                    }
                    flyout.ShowAt(listViewItem);
                }
            }
        }

        protected override void OnDetaching()
        {
            AssociatedObject.ItemClick -= AssociatedObject_ItemClick;
            AssociatedObject.RightTapped -= AssociatedObject_RightTapped;
        }

        private void AssociatedObject_ItemClick(object sender, ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            var selectedItem = e.ClickedItem;

            // Container of the selected item
            var listViewItem = listView.ContainerFromItem(selectedItem) as ListViewItem;
            if (listViewItem != null)
            {
                var flyout = FlyoutBase.GetAttachedFlyout(listViewItem);
                if (flyout is MenuFlyout menuFlyout)
                {
                    var deleteItem = menuFlyout.Items[0] as MenuFlyoutItem;
                    if (deleteItem != null)
                    {
                        deleteItem.Command = this.DeleteMessageCommand;
                        deleteItem.CommandParameter = selectedItem;
                    }

                    var copyItem = menuFlyout.Items[1] as MenuFlyoutItem;
                    if (copyItem != null)
                    {
                        copyItem.Command = this.CopyMessageCommand;
                        copyItem.CommandParameter = selectedItem;
                    }

                    var copyToPromptItem = menuFlyout.Items[2] as MenuFlyoutItem;
                    if (copyToPromptItem != null)
                    {
                        copyToPromptItem.Command = this.CopyToPromptCommand;
                        copyToPromptItem.CommandParameter = selectedItem;
                    }
                }

                flyout.ShowAt(listViewItem);
            }
        }
    }
}
