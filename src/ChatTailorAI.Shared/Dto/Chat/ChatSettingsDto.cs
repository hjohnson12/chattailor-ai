using System;
using ChatTailorAI.Shared.Models.Chat;

namespace ChatTailorAI.Shared.Dto.Chat
{
    public class ChatSettingsDto
    {
        public string Id { get; set; }
        public string ConversationId { get; set; }
        public string ModelType { get; set; }
        public string ModelSettingsJson { get; set; }

        public ChatSettingsDto()
        {
            Id = Guid.NewGuid().ToString();
        }

        public ChatSettingsDto(string conversationId, string modelType, string modelSettingsJson)
        {
            Id = Guid.NewGuid().ToString();
            ConversationId = conversationId;
            ModelType = modelType;
            ModelSettingsJson = modelSettingsJson;
        }

        public ChatSettingsDto(string id, string conversationId, string modelType, string modelSettingsJson)
        {
            Id = id;
            ConversationId = conversationId;
            ModelType = modelType;
            ModelSettingsJson = modelSettingsJson;
        }

        public ChatSettings ToEntity()
        {
            return new ChatSettings()
            {
                Id = Id,
                ConversationId = ConversationId,
                ModelType = (ModelType)Enum.Parse(typeof(ModelType), ModelType),
                ModelSettingsJson = ModelSettingsJson
            };
        }
    }
}
