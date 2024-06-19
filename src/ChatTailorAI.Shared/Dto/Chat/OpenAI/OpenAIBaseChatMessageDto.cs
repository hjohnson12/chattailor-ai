using ChatTailorAI.Shared.Models.Chat;
using ChatTailorAI.Shared.Models.Chat.OpenAI.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Dto.Chat.OpenAI
{
    public class OpenAIBaseChatMessageDto : IChatModelMessage
    {
        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("content")]
        public object Content { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("function_call", NullValueHandling = NullValueHandling.Ignore)]
        public FunctionCall FunctionCall { get; set; }
    }

    public class OpenAIUserChatMessageDto : OpenAIBaseChatMessageDto
    {
        
    }

    public class OpenAIAssistantChatMessageDto : OpenAIBaseChatMessageDto
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("tool_calls", NullValueHandling = NullValueHandling.Ignore)]
        public string ToolCalls { get; set; }

        [JsonProperty("function_call", NullValueHandling = NullValueHandling.Ignore)]
        public FunctionCall FunctionCall { get; set; }
    }
}