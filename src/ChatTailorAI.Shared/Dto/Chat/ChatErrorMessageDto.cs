using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Dto.Chat
{
    public class ChatErrorMessageDto : ChatMessageDto
    {
        public string ErrorMessage { get; set; }
    }
}
