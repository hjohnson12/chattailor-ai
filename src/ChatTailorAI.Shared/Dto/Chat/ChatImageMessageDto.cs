using ChatTailorAI.Shared.Models.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatTailorAI.Shared.Dto.Chat
{
    public class ChatImageMessageDto : ChatMessageDto
    {
        public List<ChatImageDto> Images { get; set; } = new List<ChatImageDto>();

        public ChatImageMessageDto()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
        }

        public ChatImageMessageDto(ChatImageMessage chatImageMessage) : base(chatImageMessage)
        {
            Images = chatImageMessage.Images.Select(i => new ChatImageDto(i)).ToList();
        }
    }
}
