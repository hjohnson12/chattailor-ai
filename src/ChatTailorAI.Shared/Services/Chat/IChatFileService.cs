using System.Collections.Generic;
using System.Threading.Tasks;
using ChatTailorAI.Shared.Dto.Chat;

namespace ChatTailorAI.Shared.Services.Chat
{
    public interface IChatFileService
    {
        Task SaveMessagesToFileAsync(string filename, IEnumerable<ChatMessageDto> messages);
        Task<IEnumerable<ChatMessageDto>> LoadMessagesFromFileAsync();
    }
}