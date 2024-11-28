using System.Collections.Generic;
using System.Threading.Tasks;
using ChatTailorAI.Shared.Dto.Conversations;

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