using System.Collections.Generic;
using Newtonsoft.Json;
using ChatTailorAI.Shared.Models.Chat.Anthropic.Content;
using ChatTailorAI.Shared.Models.Chat.Anthropic.Converters;

namespace ChatTailorAI.Shared.Models.Chat.Anthropic.Responses
{
    public class AnthropicMessagesResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("content")]
        [JsonConverter(typeof(AnthropicChatContentConverter))]
        public List<IAnthropicChatContent> Content { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("stop_reason")]
        public string StopReason { get; set; }

        [JsonProperty("stop_sequence")]
        public object StopSequence { get; set; } // Use object type if the value can be null or of different types

        [JsonProperty("usage")]
        public Usage Usage { get; set; }
    }

    public class Usage
    {
        [JsonProperty("input_tokens")]
        public int InputTokens { get; set; }

        [JsonProperty("output_tokens")]
        public int OutputTokens { get; set; }
    }
}