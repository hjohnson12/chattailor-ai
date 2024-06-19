using ChatTailorAI.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Common
{
    /// <summary>
    /// Interface for handling navigation within a frame.
    /// </summary>
    public interface INavigationService
    {
        object MainFrame { get; set; }
        void NavigateTo(Type type);
        void NavigateTo(Type type, object parameter);
        void NavigateTo(NavigationPageKeys pageKey);
        void NavigateTo(NavigationPageKeys pageKey, object parameter);
        void NavigateToDefaultView();
        void NavigateToSettings();
        void NavigateBack();
    }
}