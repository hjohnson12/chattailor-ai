using System;

namespace ChatTailorAI.Shared.Models.Chat
{
    public enum MessageType
    {
        Text,
        Image
    }

    public interface IChatMessage
    {
        string Id { get; set; }
        string ConversationId { get; set; }
        string Role { get; set; }
        string Content { get; set; }
        MessageType MessageType { get; set; }
        DateTime CreatedAt { get; set; }

        string ExternalMessageId { get; set; }
    }
}