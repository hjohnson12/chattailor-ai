using ChatTailorAI.Shared.Dto.Chat;
using ChatTailorAI.Shared.Models.Chat.OpenAI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Chat
{
    public interface IChatFileService
    {
        Task SaveMessagesToFileAsync(string filename, IEnumerable<ChatMessageDto> messages);
        Task<IEnumerable<ChatMessageDto>> LoadMessagesFromFileAsync();
    }
}
