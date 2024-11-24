using System.Collections.Generic;
using Newtonsoft.Json;
using ChatTailorAI.Shared.Models.Chat.Google.Content;
using ChatTailorAI.Shared.Models.Chat.Google.Converters;

namespace ChatTailorAI.Shared.Models.Chat.Google.Responses
{
    public class GoogleGenerateContentResponse
    {
        [JsonProperty("candidates", NullValueHandling = NullValueHandling.Ignore)]
        public List<Candidate> Candidates { get; set; }

        [JsonProperty("promptFeedback")]
        public PromptFeedback PromptFeedback { get; set; }
    }

    public class Candidate
    {
        [JsonProperty("content")]
        public Content Content { get; set; }

        [JsonProperty("finishReason")]
        public string FinishReason { get; set; }

        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("safetyRatings")]
        public List<SafetyRating> SafetyRatings { get; set; }
    }

    public class Content
    {
        [JsonProperty("parts")]
        [JsonConverter(typeof(GoogleChatContentConverter))]
        public List<IGoogleChatContent> Parts { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }
    }

    public class Part
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class SafetyRating
    {
        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("probability")]
        public string Probability { get; set; }
    }

    public class PromptFeedback
    {
        [JsonProperty("blockReason")]
        public string BlockReason { get; set; }

        [JsonProperty("safetyRatings")]
        public List<SafetyRating> SafetyRatings { get; set; }
    }
}