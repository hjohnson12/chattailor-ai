using ChatTailorAI.Shared.ViewModels.Dialogs;
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

namespace ChatTailorAI.Uwp.Views.Dialogs
{
    public sealed partial class NewChatDialog : ContentDialog
    {
        public NewChatDialogViewModel ViewModel => (NewChatDialogViewModel)DataContext;

        public NewChatDialog()
        {
            this.InitializeComponent();
            ChatTypeRadioButtons.SelectedIndex = 0;

            this.DataContext =
                 App.Current.ServiceHost.Services.GetService<NewChatDialogViewModel>();
        }

        private async void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.RefreshModelsAsync();
        }
    }
}