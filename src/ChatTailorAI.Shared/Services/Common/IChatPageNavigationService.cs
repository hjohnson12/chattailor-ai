using System;
using ChatTailorAI.Shared.Enums;

namespace ChatTailorAI.Shared.Services.Common
{
    /// <summary>
    /// Provides navigation capabilities when selecting a chat within the application.
    /// </summary>
    public interface IChatPageNavigationService
    {
        /// <summary>
        /// Gets or sets the chat frame used for navigation.
        /// </summary>
        object ChatFrame { get; set; }

        /// <summary>
        /// Navigates to the specified page type.
        /// </summary>
        /// <param name="type">The type of the page to navigate to.</param>
        void NavigateTo(Type type);

        /// <summary>
        /// Navigates to the specified page type with a parameter.
        /// </summary>
        /// <param name="type">The type of the page to navigate to.</param>
        /// <param name="parameter">The parameter to pass to the target page.</param>
        void NavigateTo(Type type, object parameter);

        /// <summary>
        /// Navigates to the specified page key with a parameter.
        /// </summary>
        /// <param name="pageKey">The key identifying the page to navigate to.</param>
        /// <param name="parameter">The parameter to pass to the target page.</param>
        void NavigateTo(NavigationPageKeys pageKey, object parameter);
    }
}