using ChatTailorAI.Shared.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace ChatTailorAI.Services.Uwp.Dispatching
{
    public class DispatcherService : IDispatcherService
    {
        public async Task RunOnUIThreadAsync(Action action)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                new DispatchedHandler(action)
            );
        }
    }
}
