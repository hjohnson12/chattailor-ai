using System.Collections.Generic;
using Newtonsoft.Json;

namespace ChatTailorAI.Shared.Models.Assistants.OpenAI
{
    public class OpenAIThreadMessagesResponse
    {
        [JsonProperty("object")]
        public string Object { get; set; }
        [JsonProperty("data")]
        public List<OpenAIThreadMessage> Data { get; set; }
        [JsonProperty("first_id")]
        public string FirstId { get; set; }
        [JsonProperty("last_id")]
        public string LastId { get; set; }
        [JsonProperty("has_more")]
        public bool HasMore { get; set; }
    }

    public class OpenAIThreadMessage
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("created_at")]
        public int CreatedAt { get; set; }

        [JsonProperty("thread_id")]
        public string ThreadId { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("content")]
        public List<Content> Content { get; set; } = new List<Content>();

        [JsonProperty("file_ids")]
        public List<string> FileIds { get; set; } = new List<string>();

        [JsonProperty("assistant_id")]
        public object AssistantId { get; set; } // Use 'object' to handle both null and non-null cases

        [JsonProperty("run_id")]
        public object RunId { get; set; }

        [JsonProperty("metadata")]
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
    }

    public class Content
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public TextContent Text { get; set; }
    }

    public class TextContent
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("annotations")]
        public List<object> Annotations { get; set; } = new List<object>(); // Use 'object' if annotation structure is unknown or can vary
    }
}
