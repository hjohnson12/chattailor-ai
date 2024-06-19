using ChatTailorAI.Shared.Builders;
using ChatTailorAI.Shared.Factories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Factories
{
    public class ChatRequestBuilderFactory : IChatRequestBuilderFactory
    {
        private readonly OpenAIChatRequestBuilder _openAIChatRequestBuilder;
        private readonly AnthropicChatRequestBuilder _anthropicChatRequestBuilder;
        private readonly GoogleChatRequestBuilder _googleChatRequestBuilder;
        private readonly LMStudioChatRequestBuilder _lmStudioChatRequestBuilder;

        public ChatRequestBuilderFactory(
            OpenAIChatRequestBuilder openAIChatRequestBuilder,
            AnthropicChatRequestBuilder anthropicChatRequestBuilder,
            GoogleChatRequestBuilder googleChatRequestBuilder,
            LMStudioChatRequestBuilder lmStudioChatRequestBuilder)
        {
            _openAIChatRequestBuilder = openAIChatRequestBuilder;
            _anthropicChatRequestBuilder = anthropicChatRequestBuilder;
            _googleChatRequestBuilder = googleChatRequestBuilder;
            _lmStudioChatRequestBuilder = lmStudioChatRequestBuilder;
        }

        public IChatRequestBuilder GetBuilder(string chatServiceName)
        {
            switch (chatServiceName)
            {
                case "openai":
                    return _openAIChatRequestBuilder;
                case "anthropic":
                    return _anthropicChatRequestBuilder;
                case "google":
                    return _googleChatRequestBuilder;
                case "lmstudio":
                    return _lmStudioChatRequestBuilder;
                default:
                    throw new InvalidOperationException("Unknown chat service name");
            }
        }
    }
}
