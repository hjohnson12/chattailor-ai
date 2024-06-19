using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Dto.Conversations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Factories.Interfaces
{
    public interface IDialogFactory
    {
        Task ShowDialogAsync<T>(T dialog) where T : class;
        Task<bool> ShowDeleteDialogAsync();
        Task ShowFirstRunDialogAsync();
        Task ShowAppUpdatedDialogAsync();
        Task ShowCreateAssistantDialogAsync();
        Task ShowNewChatDialogAsync();
        Task<bool> ShowEditAssistantDialogAsync(AssistantDto assistant);
        Task<bool> ShowEditChatDialogAsync(ConversationDto conversation);
        Task<bool> ShowEditPromptDialogAsync(PromptDto prompt);
        Task ShowAssistantHelpDialog();
        Task ShowChatHelpDialog();
        Task ShowGenerateImageDialogAsync();
        Task ShowCreatePromptDialogAsync();
    }
}
