using System;
using ChatTailorAI.Shared.Models.Chat.OpenAI;

namespace ChatTailorAI.Shared.Dto.Chat.OpenAI
{
    public class OpenAIChatSettingsDto : ChatSettingsDto
    {
        public double Temperature { get; set; }
        public int MaxTokens { get; set; }
        public string TopP { get; set; }
        public double FrequencyPenalty { get; set; }
        public double PresencePenalty { get; set; }
        public string StopSequences { get; set; }
        public bool Stream { get; set; }

        public OpenAIChatSettingsDto()
        {
            // TODO: define some defaults or use from settings
            Id = Guid.NewGuid().ToString();
            Temperature = 0.9;
            MaxTokens = 150;
            TopP = "1";
            FrequencyPenalty = 0.0;
            PresencePenalty = 0.6;
            StopSequences = "\n";
            Stream = false;
        }

        public new OpenAIChatSettings ToEntity()
        {
            return new OpenAIChatSettings()
            {
                Id = Id,
                Temperature = Temperature,
                MaxTokens = MaxTokens,
                TopP = TopP,
                FrequencyPenalty = FrequencyPenalty,
                PresencePenalty = PresencePenalty,
                StopSequences = StopSequences,
                Stream = Stream
            };
        }
    }
}