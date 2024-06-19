using ChatTailorAI.Shared.Services.Common;
using ChatTailorAI.Shared.Services.Events;
using ChatTailorAI.Shared.ViewModels.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ChatTailorAI.Uwp.Views
{
    public sealed partial class ShellPage : Page
    {
        public ShellPageViewModel ViewModel => (ShellPageViewModel)DataContext;

        public ShellPage()
        {
            this.InitializeComponent();

            this.DataContext =
                App.Current.ServiceHost.Services.GetService<ShellPageViewModel>();

            var navService = App.Current.ServiceHost.Services.GetRequiredService<INavigationService>();
            navService.MainFrame = ContentFrame;

            var eventAggregator = App.Current.ServiceHost.Services.GetRequiredService<IEventAggregator>();
            eventAggregator.NotificationRaised += EventAggregator_NotificationRaised;

            ContentFrame.Navigate(typeof(ChatPage));
        }

        ~ShellPage()
        {
            var eventAggregator = App.Current.ServiceHost.Services.GetRequiredService<IEventAggregator>();
            eventAggregator.NotificationRaised -= EventAggregator_NotificationRaised;
        }

        private void EventAggregator_NotificationRaised(object sender, Shared.Events.NotificationRaisedEvent e)
        {
            this.InAppNotification.Show(e.Message, e.Duration);
        }

        private void OnNavigationViewItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            if (DataContext is ShellPageViewModel viewModel)
            {
                viewModel.NavigationViewItemInvokedCommand.Execute(args.InvokedItemContainer.Tag);
            }
        }

        private void mainNav_ItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            OnNavigationViewItemInvoked(sender, args);
        }

        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null) return null;

            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return FindParent<T>(parentObject);
            }
        }

        private async void NavigationViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (sender is Microsoft.UI.Xaml.Controls.NavigationViewItem navViewItem)
            {
                if (navViewItem.Tag?.ToString() == "PictureInPicture")
                {
                    var view = ApplicationView.GetForCurrentView();

                    if (view.ViewMode != ApplicationViewMode.CompactOverlay)
                    {
                        await view.TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay);
                    }
                    else
                    {
                        await view.TryEnterViewModeAsync(ApplicationViewMode.Default);
                    }
                }
            }
        }

        //private async void mainNav_ItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        //{
        //    if (args.IsSettingsInvoked)
        //    {
        //        // Handle settings if needed
        //    }
        //    else if (args.InvokedItemContainer != null)
        //    {
        //        switch (args.InvokedItemContainer.Tag.ToString())
        //        {
        //            case "CreateAssistant":
        //                await ShowCreateAssistantDialog();
        //                break;
        //            case "NewChat":
        //                await ShowNewChatDialog();
        //                break;
        //            default:
        //                NavigateToPage(args.InvokedItemContainer.Tag.ToString());
        //                break;
        //        }
        //    }
        //}
    }
}
