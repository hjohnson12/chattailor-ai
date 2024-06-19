using System;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using ChatTailorAI.Shared.Services.Common;

namespace ChatTailorAI.Services.Uwp.UI.Views
{
    public class ApplicationViewService : IApplicationViewService
    {
        public async Task<bool> TryEnterPictureInPictureModeAsync()
        {
            var view = ApplicationView.GetForCurrentView();

            if (view.IsViewModeSupported(ApplicationViewMode.CompactOverlay))
            {
                if (view.ViewMode != ApplicationViewMode.CompactOverlay)
                {
                    return await view.TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay);
                }
                else
                {
                    return await view.TryEnterViewModeAsync(ApplicationViewMode.Default);
                }
            }

            return false;
        }
    }
}