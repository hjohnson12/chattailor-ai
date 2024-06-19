using ChatTailorAI.Shared.ViewModels.Dialogs;
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

namespace ChatTailorAI.Uwp.Views.Dialogs
{
    public sealed partial class EditChatDialog : ContentDialog
    {
        public EditChatDialog()
        {
            this.InitializeComponent();
        }

        public EditChatDialogViewModel ViewModel => (EditChatDialogViewModel)DataContext;

        private async void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.RefreshModelsAsync();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
