using ChatTailorAI.Shared.Models.Chat;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Dto.Chat.LMStudio
{
    public class LMStudioBaseChatMessageDto : IChatModelMessage
    {
        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("content")]
        public object Content { get; set; }
    }

    public class LMStudioUserChatMessageDto : LMStudioBaseChatMessageDto
    {

    }

    public class LMStudioAssistantChatMessageDto : LMStudioBaseChatMessageDto
    {

    }
}