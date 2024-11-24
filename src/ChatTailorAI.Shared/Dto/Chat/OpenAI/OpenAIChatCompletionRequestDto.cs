using System.Collections.Generic;
using ChatTailorAI.Shared.Models.Tools;

namespace ChatTailorAI.Shared.Dto.Chat.OpenAI
{
    public class OpenAIChatCompletionRequestDto
    {
        public List<OpenAIBaseChatMessageDto> Messages { get; set; }
        public string Model { get; set; }
        public double? FrequencyPenalty { get; set; }
        public Dictionary<int, double> LogitBias { get; set; }
        public bool? LogProbs { get; set; }
        public int? TopLogProbs { get; set; }
        public int? MaxTokens { get; set; }
        public int? N { get; set; }
        public double? PresencePenalty { get; set; }
        //public ResponseFormat ResponseFormat { get; set; }
        public int? Seed { get; set; }
        //public dynamic Stop { get; set; } 
        public bool? Stream { get; set; }
        public double? Temperature { get; set; }
        public double? TopP { get; set; }
        public List<Tool> Tools { get; set; }
        //public ToolChoice ToolChoice { get; set; }
        public string User { get; set; }
    }
}
