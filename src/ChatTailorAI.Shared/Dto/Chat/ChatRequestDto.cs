using System.Collections.Generic;

namespace ChatTailorAI.Shared.Dto.Chat
{
    public class ChatRequestDto<TSettingsDto, TMessageDto>
    {
        public string Model { get; set; }

        public string Instructions { get; set; }

        public List<TMessageDto> Messages { get; set; }

        public TSettingsDto Settings { get; set; }
    }
}