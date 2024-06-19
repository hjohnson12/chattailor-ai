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
    public sealed partial class DeleteMessagesDialog : ContentDialog
    {
        public bool IsDeleteConfirmed { get; private set; }

        public DeleteMessagesDialog()
        {
            this.InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            IsDeleteConfirmed = false;
            this.Hide();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            IsDeleteConfirmed = true;
            this.Hide();
        }
    }
}