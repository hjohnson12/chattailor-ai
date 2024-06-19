using ChatTailorAI.Shared.Dto.Chat.OpenAI;
using ChatTailorAI.Shared.Models.Chat.OpenAI;
using ChatTailorAI.Shared.Models.Settings;
using ChatTailorAI.Shared.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatTailorAI.Shared.Builders
{
    public class OpenAIChatRequestBuilder : IChatRequestBuilder
    {
        public object BuildChatRequest(string model, string instructions, IEnumerable<object> messages, object settings)
        {
            var openAIMessages = messages.Cast<OpenAIBaseChatMessageDto>();
            var openAISettings = (OpenAIChatSettings)settings;

            var openAIChatRequest = new OpenAIChatRequest
            {
                Model = model,
                Instructions = instructions,
                Messages = openAIMessages.ToList(),
                Settings = openAISettings
            };

            return openAIChatRequest;
        }
    }

    //public class OpenAIChatRequestBuilder 
    //    : IChatRequestBuilder<OpenAIChatRequest, OpenAIBaseChatMessageDto, OpenAIChatSettings>
    //{
    //    public OpenAIChatRequest BuildChatRequest(string model, string instructions, IEnumerable<OpenAIBaseChatMessageDto> messages, OpenAIChatSettings settings)
    //    {
    //        var openAIChatRequest = new OpenAIChatRequest
    //        {
    //            Model = model,
    //            Instructions = instructions,
    //            Messages = messages.ToList(),
    //            Settings = settings
    //        };

    //        return openAIChatRequest;
    //    }
    //}

    //public class OpenAIChatRequestBuilder : IChatRequestBuilder<OpenAIChatRequest, OpenAIBaseChatMessageDto, OpenAIChatSettings>
    //{
    //    public OpenAIChatRequest BuildChatRequest(string model, string instructions, IEnumerable<OpenAIBaseChatMessageDto> messages, OpenAIChatSettings settings)
    //    {
    //        var openAIChatRequest = new OpenAIChatRequest
    //        {
    //            Model = model,
    //            Instructions = instructions,
    //            Messages = messages.ToList(),
    //            Settings = settings
    //        };

    //        return openAIChatRequest;
    //    }
    //}
}
