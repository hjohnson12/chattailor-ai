using Newtonsoft.Json;

namespace ChatTailorAI.Shared.Models.Chat.Google.Content
{
    public class GoogleTextContentPart : IGoogleChatContent
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}