using ChatTailorAI.Shared.Dto.Chat;
using ChatTailorAI.Shared.ViewModels;

namespace ChatTailorAI.Shared.Factories.Interfaces
{
    /// <summary>
    /// Factory for creating ChatMessageViewModels
    /// </summary>
    public interface IChatMessageViewModelFactory
    {
        /// <summary>
        /// Creates a ChatMessageViewModel from a ChatMessageDto
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ChatMessageViewModel CreateViewModelFromDto(ChatMessageDto dto);

        /// <summary>
        /// Creates a ChatMessageViewModel from a ChatMessageDto
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dto"></param>
        /// <returns></returns>
        T CreateViewModel<T>(ChatMessageDto dto) where T : ChatMessageViewModel;
    }
}