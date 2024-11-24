using System.Collections.Generic;
using Newtonsoft.Json;

namespace ChatTailorAI.Shared.Models.Chat.Anthropic.Events
{
    public abstract class StreamEvent
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class MessageStartEvent : StreamEvent
    {
        [JsonProperty("message")]
        public MessageDetails Message { get; set; }

        public class MessageDetails
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; } // Redefining Type to differentiate from StreamEvent

            [JsonProperty("role")]
            public string Role { get; set; }

            [JsonProperty("content")]
            public List<ContentDetail> Content { get; set; }

            [JsonProperty("model")]
            public string Model { get; set; }

            [JsonProperty("stop_reason")]
            public string StopReason { get; set; }

            [JsonProperty("stop_sequence")]
            public object StopSequence { get; set; }

            [JsonProperty("usage")]
            public UsageDetails Usage { get; set; }
        }
    }

    public class ContentBlockStartEvent : StreamEvent
    {
        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("content_block")]
        public ContentBlock ContentBlock { get; set; }
    }

    public class PingEvent : StreamEvent
    {
        // Ping event might not carry additional data specific to its function
    }

    public class ContentBlockDeltaEvent : StreamEvent
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("delta")]
        public DeltaContent Delta { get; set; }

        public class DeltaContent
        {
            [JsonProperty("type")]
            public string Type { get; set; } // Assuming this is "text_delta"

            [JsonProperty("text")]
            public string Text { get; set; }
        }
    }

    public class ContentBlockStopEvent : StreamEvent
    {
        [JsonProperty("index")]
        public int Index { get; set; }
    }

    public class MessageDeltaEvent : StreamEvent
    {
        [JsonProperty("delta")]
        public MessageDelta Delta { get; set; }
    }

    public class MessageStopEvent : StreamEvent
    {
        // Message stop event might not carry additional data specific to its function
    }

    // Additional classes based on the event types' requirements
    public class ContentBlock
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class MessageDelta
    {
        [JsonProperty("stop_reason")]
        public string StopReason { get; set; }

        [JsonProperty("stop_sequence")]
        public object StopSequence { get; set; }

        [JsonProperty("usage")]
        public UsageDetails Usage { get; set; }
    }

    public class UsageDetails
    {
        [JsonProperty("input_tokens")]
        public int InputTokens { get; set; }

        [JsonProperty("output_tokens")]
        public int OutputTokens { get; set; }
    }

    public class ContentDetail
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
