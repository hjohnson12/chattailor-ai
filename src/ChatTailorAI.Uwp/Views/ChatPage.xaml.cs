using System;
using System.Web;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Dto.Conversations;
using ChatTailorAI.Shared.Services.Events;
using ChatTailorAI.Shared.Events;
using ChatTailorAI.Shared.ViewModels.Pages;

namespace ChatTailorAI.Uwp.Views
{
    public sealed partial class ChatPage : Page
    {
        public ChatPageViewModel ChatViewModel => (ChatPageViewModel)DataContext;

        private readonly IEventAggregator _eventAggregator;

        public ChatPage()
        {
            this.InitializeComponent();
            
            this.DataContext = 
                App.Current.ServiceHost.Services.GetService<ChatPageViewModel>();

            _eventAggregator = App.Current.ServiceHost.Services.GetService<IEventAggregator>();
            _eventAggregator.AuthenticationRequested += OnAuthenticationRequested;
        }

        private async void OnAuthenticationRequested(object sender, AuthenticationRequestedEvent e)
        {

            if (Dispatcher.HasThreadAccess)
            {
                await webView.EnsureCoreWebView2Async();
                webView.CoreWebView2.CookieManager.DeleteAllCookies();
                webView.Source = new Uri(e.AuthenticationUrl);
                webView.Visibility = Visibility.Visible;
            }
            else
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    await webView.EnsureCoreWebView2Async();
                    webView.CoreWebView2.CookieManager.DeleteAllCookies();
                    webView.Source = new Uri(e.AuthenticationUrl);
                    webView.Visibility = Visibility.Visible;
                });
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            ChatViewModel.Dispose();

            _eventAggregator.AuthenticationRequested -= OnAuthenticationRequested;
        }

        public void NavigationCleanup()
        {
            ChatViewModel.Dispose();
            _eventAggregator.AuthenticationRequested -= OnAuthenticationRequested;
        } 

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
           base.OnNavigatedTo(e);

            ChatViewModel.IsActiveViewModel = true;

            switch (e.Parameter)
            {
                case AssistantDto assistantDto:
                    // Assistant was selected to chat with from assistants page
                    ChatViewModel.InitializeChat(assistantDto);
                    break;
                case ConversationDto conversation:
                    // Conversation was selected from the chat list or new convo was created
                    // Note: Can be either a normal chat or a chat with an assistant
                    ChatViewModel.InitializeChat(conversation);
                    await ChatViewModel.LoadMessages();
                    break;
                case PromptDto promptDto:
                    await ChatViewModel.InitializeInstantChat(promptDto);
                    break;
                default:
                    // Instant chat selected, creates with default settings
                    await ChatViewModel.InitializeInstantChat();
                    break;
            }
        }

        private async void PiPButton_Click(object sender, RoutedEventArgs e)
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

        private async void InputTextBox_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                var shiftState = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Shift);
                if (shiftState.HasFlag(CoreVirtualKeyStates.Down))
                {
                    // Add newline only if shift and enter are down
                    var textBox = (TextBox)sender;
                    var selectionStart = textBox.SelectionStart;
                    textBox.Text = textBox.Text.Insert(selectionStart, Environment.NewLine);
                    textBox.SelectionStart = selectionStart + Environment.NewLine.Length;
                }
                else if (ChatViewModel.IsTyping)
                {
                    e.Handled = true;
                }
                else
                {
                    e.Handled = true;
                    await ChatViewModel.SendMessage();
                }
            }
        }

        private void ChatListView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            //if (args.ItemIndex == ChatViewModel.Messages.Count - 1)
            //{
            //    sender.ScrollIntoView(args.Item);
            //}
        }

        private async void exportButton_Click(object sender, RoutedEventArgs e)
        {
            await ChatViewModel.ExportChatToFile();
        }

        private async void importButton_Click(object sender, RoutedEventArgs e)
        {
            await ChatViewModel.ImportChatFromFile();
        }

        private static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(child);
            if (parent == null)
                return null;

            if (parent is T parentAsT)
                return parentAsT;
            else
                return FindParent<T>(parent);
        }

        private async void WebView_NavigationStarting(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs args)
        {
            Uri uri = new Uri(args.Uri);
            if (uri.Host.Equals("localhost") && uri.Port == 5000)
            {
                sender.Visibility = Visibility.Collapsed;
                var queryItems = HttpUtility.ParseQueryString(uri.Query);
                var authCode = queryItems.Get("code");

                args.Cancel = true;

                // Navigate to a blank page
                await webView.EnsureCoreWebView2Async();

                // Pass the auth code back to the ViewModel to get the access token
                var tokenObtained = await ChatViewModel.RequestAccessToken(authCode);

                ChatViewModel.AuthenticationComplete(tokenObtained);
            }
        }

        private async void MainView_Loaded(object sender, RoutedEventArgs e)
        {
            await ChatViewModel.InitializeMediaCapture();
        }
    }
}