using System.Threading.Tasks;
using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Dto.Conversations;

namespace ChatTailorAI.Shared.Factories.Interfaces
{
    /// <summary>
    /// Factory interface for creating dialogs.
    /// </summary>
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
