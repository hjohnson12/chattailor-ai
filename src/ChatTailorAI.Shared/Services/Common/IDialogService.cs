using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Dto.Conversations;
using ChatTailorAI.Shared.Models.Assistants;
using ChatTailorAI.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Common
{
    /// <summary>
    /// Interface that provides methods for displaying dialogs.
    /// </summary>
    public interface IDialogService
    {
        bool CheckForOpenDialog();
        Task<bool> ShowDeleteDialogAsync();
        Task ShowFirstRunDialogAsync();
        Task ShowAppUpdatedDialogAsync();
        Task ShowCreateAssistantDialogAsync();
        Task ShowNewChatDialogAsync();
        Task<bool> ShowEditAssistantDialogAsync(AssistantDto assistant);
        Task<bool> ShowEditChatDialogAsync(ConversationDto conversation);
        Task<bool> ShowEditPromptDialogAsync(PromptDto prompt);
        Task ShowAssistantHelpDialogAsync();
        Task ShowChatHelpDialogAsync();
        Task ShowGenerateImageDialogAsync();
        Task ShowCreatePromptDialogAsync();
    }
}
