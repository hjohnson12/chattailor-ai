using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

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
