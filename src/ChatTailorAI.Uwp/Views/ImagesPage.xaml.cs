using ChatTailorAI.Shared.ViewModels.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ChatTailorAI.Uwp.Views
{
    public sealed partial class ImagesPage : Page
    {
        public ImagesPageViewModel ViewModel => (ImagesPageViewModel)DataContext;

        public ImagesPage()
        {
            this.InitializeComponent();

            this.DataContext =
                App.Current.ServiceHost.Services.GetService<ImagesPageViewModel>();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // TODO: Optimize image loading
            // Navigation has a slight delay when navigating to this page
            //await ViewModel.LoadImages();

            await Dispatcher.RunAsync(CoreDispatcherPriority.Low, async () =>
            {
                await ViewModel.LoadImages();
            });
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            ViewModel.Dispose();
        }
    }
}
