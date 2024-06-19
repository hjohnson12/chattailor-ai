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
    public sealed partial class AssistantHelpDialog : ContentDialog
    {
        public AssistantHelpDialog()
        {
            this.InitializeComponent();
        }

        private async void Hyperlink_Click(Windows.UI.Xaml.Documents.Hyperlink sender, Windows.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            var assistantsUri = new Uri("https://platform.openai.com/assistants");
            await Windows.System.Launcher.LaunchUriAsync(assistantsUri);
        }
    }
}
