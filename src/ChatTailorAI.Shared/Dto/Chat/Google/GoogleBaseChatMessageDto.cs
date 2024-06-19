using ChatTailorAI.Shared.Dto.Chat.Anthropic;
using ChatTailorAI.Shared.Models.Chat;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Dto.Chat.Google
{
    public class GoogleBaseChatMessageDto : IChatModelMessage
    {
        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("content", NullValueHandling = NullValueHandling.Ignore)]
        public object Content { get; set; }

        [JsonProperty("parts", NullValueHandling = NullValueHandling.Ignore)]
        public object Parts { get; set; }
    }

    public class GoogleUserChatMessageDto : GoogleBaseChatMessageDto
    {

    }

    public class GoogleAssistantChatMessageDto : GoogleBaseChatMessageDto
    {

    }
}