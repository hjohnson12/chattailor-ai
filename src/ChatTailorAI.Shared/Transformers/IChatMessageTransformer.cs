using System.Threading.Tasks;
using ChatTailorAI.Shared.Dto.Chat;
using ChatTailorAI.Shared.Models.Chat;

namespace ChatTailorAI.Shared.Transformers
{
    /// <summary>
    /// An interface for transforming chat messages from DTOs to model messages.
    /// </summary>
    public interface IChatMessageTransformer
    {
        /// <summary>
        /// Transforms a chat message DTO into a chat model message.
        /// </summary>
        /// <param name="messageDto"></param>
        /// <returns></returns>
        Task<IChatModelMessage> Transform(ChatMessageDto messageDto);
    }
}