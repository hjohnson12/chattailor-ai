using ChatTailorAI.Shared.Dto.Conversations;
using ChatTailorAI.Shared.Models.Conversations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.DataServices
{
    public interface IConversationDataService
    {
        Task SaveConversationAsync(ConversationDto conversation);
        Task<IEnumerable<ConversationDto>> GetConversationsAsync();
        Task DeleteConversationAsync(ConversationDto conversation);
        Task UpdateConversationAsync(ConversationDto conversation);
    }
}
