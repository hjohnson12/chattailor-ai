using System.Threading.Tasks;
using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Dto.Conversations;
using ChatTailorAI.Shared.Factories.Interfaces;
using ChatTailorAI.Shared.Services.Common;

namespace ChatTailorAI.Uwp.Services.UI.Dialogs
{
    /// <summary>
    /// Provides a service for opening specific dialogs within the application.
    /// </summary>
    public class DialogService : IDialogService
    {
        private readonly IDialogFactory _dialogFactory;

        public DialogService(IDialogFactory dialogFactory)
        {
            _dialogFactory = dialogFactory;
        }

        public async Task<bool> ShowDeleteDialogAsync()
        {
            return await _dialogFactory.ShowDeleteDialogAsync();
        }

        public async Task ShowFirstRunDialogAsync()
        {
            await _dialogFactory.ShowFirstRunDialogAsync();
        }

        public async Task ShowAppUpdatedDialogAsync()
        {
            await _dialogFactory.ShowAppUpdatedDialogAsync();
        }

        public async Task ShowCreateAssistantDialogAsync()
        {
            await _dialogFactory.ShowCreateAssistantDialogAsync();
        }

        public async Task ShowNewChatDialogAsync()
        {
            await _dialogFactory.ShowNewChatDialogAsync();
        }

        public async Task<bool> ShowEditAssistantDialogAsync(AssistantDto assistant)
        {
            return await _dialogFactory.ShowEditAssistantDialogAsync(assistant);
        }

        public async Task<bool> ShowEditChatDialogAsync(ConversationDto conversation)
        {
            return await _dialogFactory.ShowEditChatDialogAsync(conversation);
        }

        public async Task<bool> ShowEditPromptDialogAsync(PromptDto prompt)
        {
            return await _dialogFactory.ShowEditPromptDialogAsync(prompt);
        }

        public async Task ShowAssistantHelpDialogAsync()
        {
            await _dialogFactory.ShowAssistantHelpDialog();
        }

        public async Task ShowChatHelpDialogAsync()
        {
            await _dialogFactory.ShowChatHelpDialog();
        }

        public async Task ShowGenerateImageDialogAsync()
        {
            await _dialogFactory.ShowGenerateImageDialogAsync();
        }

        public async Task ShowCreatePromptDialogAsync()
        {
            await _dialogFactory.ShowCreatePromptDialogAsync();
        }
    }
}