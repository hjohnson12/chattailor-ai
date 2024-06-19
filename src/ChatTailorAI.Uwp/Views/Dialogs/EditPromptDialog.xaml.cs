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
    public sealed partial class EditPromptDialog : ContentDialog
    {
        public EditPromptDialogViewModel ViewModel => (EditPromptDialogViewModel)DataContext;
        public EditPromptDialog()
        {
            this.InitializeComponent();

            this.DataContext =
                 App.Current.ServiceHost.Services.GetService<EditPromptDialogViewModel>();
        }
    }
}
