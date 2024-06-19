using ChatTailorAI.Shared.Factories.Interfaces;
using ChatTailorAI.Shared.Services.Files;
using ChatTailorAI.Shared.Transformers;
using ChatTailorAI.Shared.Transformers.Anthropic;
using ChatTailorAI.Shared.Transformers.Google;
using ChatTailorAI.Shared.Transformers.LMStudio;
using ChatTailorAI.Shared.Transformers.OpenAI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Factories
{
    public class ChatMessageTransformerFactory : IChatMessageTransformerFactory
    {
        private readonly OpenAIChatMessageTransformer _gptMessageTransformer;
        private readonly AnthropicChatMessageTransformer _anthropicMessageTransformer;
        private readonly GoogleChatMessageTransformer _geminiMessageTransformer;
        private readonly LMStudioChatMessageTransformer _lmStudioChatMessageTransformer;

        public ChatMessageTransformerFactory(
            OpenAIChatMessageTransformer openAIGptMessageTransformer,
            AnthropicChatMessageTransformer anthropicClaudeChatMessageTransformer,
            GoogleChatMessageTransformer googleGeminiChatMessageTransformer,
            LMStudioChatMessageTransformer lMStudioChatMessageTransformer)
        {
            _gptMessageTransformer = openAIGptMessageTransformer;
            _anthropicMessageTransformer = anthropicClaudeChatMessageTransformer;
            _geminiMessageTransformer = googleGeminiChatMessageTransformer;
            _lmStudioChatMessageTransformer = lMStudioChatMessageTransformer;
        }

        public IChatMessageTransformer Create(string chatServiceName)
        {
            switch (chatServiceName)
            {
                case "openai":
                    return _gptMessageTransformer;
                case "anthropic":
                    return _anthropicMessageTransformer;
                case "google":
                    return _geminiMessageTransformer;
                case "lmstudio":
                    return _lmStudioChatMessageTransformer;
                default:
                    throw new InvalidOperationException("Unknown chat service name");
            }
        }
    }
}
