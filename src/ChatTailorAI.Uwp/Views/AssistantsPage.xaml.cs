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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AssistantsPage : Page
    {
        public AssistantsPageViewModel ViewModel => (AssistantsPageViewModel)DataContext;

        public AssistantsPage()
        {
            this.InitializeComponent();

            this.DataContext =
                App.Current.ServiceHost.Services.GetService<AssistantsPageViewModel>();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.Dispose();
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

        private void Grid_GotFocus(object sender, RoutedEventArgs e)
        {
            var grid = sender as Grid;
            if (grid != null)
            {
                var gridViewItem = FindParent<GridViewItem>(grid);
                if (gridViewItem != null)
                {
                    gridViewItem.IsSelected = true;
                    gridViewItem.Focus(FocusState.Programmatic);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Note: This is a workaround for the below WinUI issue: 
            // https://github.com/microsoft/microsoft-ui-xaml/issues/6350
            (sender as Button).ReleasePointerCaptures();
        }
    }
}