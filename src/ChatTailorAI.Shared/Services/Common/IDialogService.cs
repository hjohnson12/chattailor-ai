using System.Threading.Tasks;
using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Dto.Conversations;

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
