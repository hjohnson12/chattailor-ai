using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using ChatTailorAI.Shared.Services.Common;

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
