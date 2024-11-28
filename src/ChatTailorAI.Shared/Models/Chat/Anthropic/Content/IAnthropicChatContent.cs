using Newtonsoft.Json;

namespace ChatTailorAI.Shared.Models.Chat.Anthropic.Content
{
    public interface IAnthropicChatContent
    {
        [JsonProperty("type")]
        string Type { get; }
    }
}