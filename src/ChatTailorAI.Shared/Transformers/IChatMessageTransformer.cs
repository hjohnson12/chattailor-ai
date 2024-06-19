using ChatTailorAI.Shared.Dto.Chat;
using ChatTailorAI.Shared.Models.Chat;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Transformers
{
    public interface IChatMessageTransformer
    {
        Task<IChatModelMessage> Transform(ChatMessageDto messageDto);
    }
}