using ChatTailorAI.Shared.Dto.Chat;
using ChatTailorAI.Shared.Factories.Interfaces;
using ChatTailorAI.Shared.Models.Chat;
using ChatTailorAI.Shared.Services.Chat.OpenAI;
using ChatTailorAI.Shared.Services.Chat;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Factories
{
    public class ChatServiceFactory : IChatServiceFactory
    {
        private readonly OpenAIChatService _openAiChatService;

        public ChatServiceFactory(OpenAIChatService openAiChatService)
        {
            _openAiChatService = openAiChatService;
        }

        public IChatService<TSettings, TMessage, TResponse> Create<TSettings, TMessage, TResponse>(string key)
            where TSettings : ChatSettings
            where TMessage : ChatMessageDto
            where TResponse : ChatMessageDto
        {
            switch (key)
            {
                case "OpenAI":
                    if (_openAiChatService is IChatService<TSettings, TMessage, TResponse> openAiService)
                    {
                        return openAiService;
                    }

                    throw new InvalidCastException($"The OpenAIChatService does not support the requested types {typeof(TSettings).Name}, {typeof(TMessage).Name}, {typeof(TResponse).Name}.");
                default:
                    throw new KeyNotFoundException($"Chat service with the key '{key}' was not found.");
            }
        }
    }
}
