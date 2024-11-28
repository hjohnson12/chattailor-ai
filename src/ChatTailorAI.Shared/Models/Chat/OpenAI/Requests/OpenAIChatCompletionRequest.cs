using System.Collections.Generic;
using Newtonsoft.Json;
using ChatTailorAI.Shared.Dto.Chat.OpenAI;

namespace ChatTailorAI.Shared.Models.Chat.OpenAI.Requests
{
    public class Parameter
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("properties")]
        public Dictionary<string, ParameterProperties> Properties { get; set; }

        [JsonProperty(PropertyName = "required", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Required { get; set; }
    }

    public class ParameterProperties
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public class RootObject
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("parameters")]
        public Parameter Parameters { get; set; }
    }

    public class OpenAIChatCompletionRequest
    {
        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("messages")]
        public List<OpenAIBaseChatMessageDto> Messages { get; set; }

        [JsonProperty("max_completion_tokens")]
        public int MaxCompletionTokens { get; set; }

        [JsonProperty("n")]
        public int N { get; set; }

        [JsonProperty("stop", NullValueHandling = NullValueHandling.Ignore)]
        public string Stop { get; set; }
        [JsonProperty("stream", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Stream { get; set; }

        [JsonProperty(PropertyName = "functions", NullValueHandling = NullValueHandling.Ignore)]
        public List<RootObject> Functions { get; set; }

        [JsonProperty(PropertyName = "function_call", NullValueHandling = NullValueHandling.Ignore)]
        public string FunctionCall { get; set; }

        [JsonProperty("frequency_penalty", NullValueHandling = NullValueHandling.Ignore)]
        public double? FrequencyPenalty { get; set; }

        [JsonProperty("presence_penalty", NullValueHandling = NullValueHandling.Ignore)]
        public double? PresencePenalty { get; set; }

        [JsonProperty("temperature", NullValueHandling = NullValueHandling.Ignore)]
        public double? Temperature { get; set; }
    }
}
