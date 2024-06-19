using ChatTailorAI.Shared.Dto.Conversations;
using ChatTailorAI.Shared.Models.Chat;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Dto.Chat
{
    public class ChatMessageDto
    {
        public string Id { get; set; }
        public string ConversationId { get; set; }
        public string Role { get; set; }
        public string Content { get; set; }
        public MessageType MessageType { get; set; }
        public DateTime CreatedAt { get; set; }

        public string ExternalMessageId { get; set; }

        public string ErrorMessage { get; set; }

        public ChatMessageDto()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
        }

        public ChatMessageDto(ChatMessage message)
        {
            Id = message.Id;
            ConversationId = message.ConversationId;
            Role = message.Role;
            Content = message.Content;
            MessageType = message.MessageType;
            CreatedAt = message.CreatedAt;
            ExternalMessageId = message.ExternalMessageId;
        }

        public static ChatMessageDto FromChatMessage(ChatMessage chatMessage)
        {
            return new ChatMessageDto(chatMessage);
        }

        public void UpdateEntity(ChatMessage chatMessage)
        {
            chatMessage.Id = Id;
            chatMessage.ConversationId = ConversationId;
            chatMessage.Role = Role;
            chatMessage.Content = Content;
            chatMessage.MessageType = MessageType;
            chatMessage.CreatedAt = CreatedAt;
            chatMessage.ExternalMessageId = ExternalMessageId;
        }
    }
}