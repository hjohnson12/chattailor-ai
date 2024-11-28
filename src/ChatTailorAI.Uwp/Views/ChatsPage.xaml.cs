using ChatTailorAI.Shared.Models.Conversations;
using ChatTailorAI.Shared.Services.Common.Navigation;
using ChatTailorAI.Shared.ViewModels;
using ChatTailorAI.Shared.ViewModels.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ChatTailorAI.Uwp.Views
{
    public sealed partial class ChatsPage : Page
    {
        public ChatsPageViewModel ViewModel => (ChatsPageViewModel)DataContext;

        public ChatsPage()
        {
            this.InitializeComponent();

            this.DataContext =
                App.Current.ServiceHost.Services.GetService<ChatsPageViewModel>();

            var navigationService = App.Current.ServiceHost.Services.GetService<IChatPageNavigationService>();
            navigationService.ChatFrame = ContentFrame;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);

            if (ContentFrame.Content is ChatPage chatPage)
            {
                chatPage.NavigationCleanup();

                //if (chatPage.DataContext is IDisposable disposableViewModel)
                //{
                //    disposableViewModel.Dispose();
                //}

                ContentFrame.Content = null;
            }

            ViewModel.Dispose();
        }

        private void ChatsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedChat = e.ClickedItem as ConversationViewModel;

            ContentFrame.Navigate(typeof(ChatPage), clickedChat.ToDto());
        }

        private void HidePaneButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsChatsPaneOpen)
            {
                ViewModel.IsChatsPaneOpen = false;
            }
            else
            {
                ViewModel.IsChatsPaneOpen = true;
            }
        }
    }
}