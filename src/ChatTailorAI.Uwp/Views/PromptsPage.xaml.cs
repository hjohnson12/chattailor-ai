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
    public sealed partial class PromptsPage : Page
    {
        public PromptsPage()
        {
            this.InitializeComponent();

            this.DataContext =
                App.Current.ServiceHost.Services.GetService<PromptsPageViewModel>();
        }

        public PromptsPageViewModel ViewModel => (PromptsPageViewModel)DataContext;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Note: This is a workaround for the below WinUI issue: 
            // https://github.com/microsoft/microsoft-ui-xaml/issues/6350
            (sender as Button).ReleasePointerCaptures();
        }
    }
}
