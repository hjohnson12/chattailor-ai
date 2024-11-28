using ChatTailorAI.Shared.Models.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ChatTailorAI.Shared.Models.Assistants.OpenAI
{
    public class OpenAIThreadRun
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("created_at")]
        public long CreatedAtUnix { get; set; }

        [JsonProperty("assistant_id")]
        public string AssistantId { get; set; }

        [JsonProperty("thread_id")]
        public string ThreadId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("started_at")]
        public long? StartedAtUnix { get; set; }

        [JsonProperty("expires_at")]
        public long? ExpiresAtUnix { get; set; }

        [JsonProperty("cancelled_at")]
        public long? CancelledAtUnix { get; set; }

        [JsonProperty("failed_at")]
        public long? FailedAtUnix { get; set; }

        [JsonProperty("completed_at")]
        public long? CompletedAtUnix { get; set; }

        [JsonProperty("last_error")]
        public string LastError { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("instructions")]
        public string Instructions { get; set; }

        [JsonProperty("tools")]
        public List<Tool> Tools { get; set; }

        [JsonProperty("file_ids")]
        public List<string> FileIds { get; set; }

        [JsonProperty("metadata")]
        public Dictionary<string, object> Metadata { get; set; }

        // Helper properties to convert Unix timestamp to DateTime
        public DateTime? CreatedAt => UnixTimeStampToDateTime(CreatedAtUnix);
        public DateTime? StartedAt => UnixTimeStampToDateTime(StartedAtUnix);
        public DateTime? ExpiresAt => UnixTimeStampToDateTime(ExpiresAtUnix);
        public DateTime? CancelledAt => UnixTimeStampToDateTime(CancelledAtUnix);
        public DateTime? FailedAt => UnixTimeStampToDateTime(FailedAtUnix);
        public DateTime? CompletedAt => UnixTimeStampToDateTime(CompletedAtUnix);

        private DateTime? UnixTimeStampToDateTime(long? unixTimeStamp)
        {
            if (!unixTimeStamp.HasValue) return null;
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp.Value);
            return dateTimeOffset.UtcDateTime;
        }
    }
}
