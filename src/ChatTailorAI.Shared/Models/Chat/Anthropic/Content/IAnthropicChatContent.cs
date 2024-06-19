using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Models.Chat.Anthropic.Content
{
    public interface IAnthropicChatContent
    {
        [JsonProperty("type")]
        string Type { get; }
    }
}