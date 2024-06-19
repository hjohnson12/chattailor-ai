using ChatTailorAI.Shared.Dto.Chat;
using ChatTailorAI.Shared.Models.Chat;
using ChatTailorAI.Shared.Services.Chat;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Factories.Interfaces
{
    public interface IChatServiceFactory
    {
        IChatService<TSettings, TMessage, TResponse> Create<TSettings, TMessage, TResponse>(string key)
            where TSettings : ChatSettings
            where TMessage : ChatMessageDto
            where TResponse : ChatMessageDto;
    }
}
