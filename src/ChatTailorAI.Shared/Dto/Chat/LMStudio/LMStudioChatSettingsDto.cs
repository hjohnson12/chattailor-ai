using System;
using ChatTailorAI.Shared.Models.Chat.LMStudio;

namespace ChatTailorAI.Shared.Dto.Chat.LMStudio
{
    public class LMStudioChatSettingsDto : ChatSettingsDto
    {
        public double Temperature { get; set; }
        public int MaxTokens { get; set; }
        public bool Stream { get; set; }

        public LMStudioChatSettingsDto()
        {
            // TODO: define some defaults or use from settings
            Id = Guid.NewGuid().ToString();
            Temperature = 0.9;
            MaxTokens = 150;
            Stream = false;
        }

        public new LMStudioChatSettings ToEntity()
        {
            return new LMStudioChatSettings()
            {
                Id = Id,
                Temperature = Temperature,
                MaxTokens = MaxTokens,
                Stream = Stream
            };
        }
    }
}
