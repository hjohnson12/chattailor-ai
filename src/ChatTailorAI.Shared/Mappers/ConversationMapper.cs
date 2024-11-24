using ChatTailorAI.Shared.Dto.Conversations;
using ChatTailorAI.Shared.Dto.Conversations.OpenAI;
using ChatTailorAI.Shared.Mappers.Interfaces;
using ChatTailorAI.Shared.Models.Conversations;
using ChatTailorAI.Shared.Models.Conversations.OpenAI;

namespace ChatTailorAI.Shared.Mappers
{
    public class ConversationMapper : IConversationMapper
    {
        public ConversationDto MapToDto(Conversation conversation)
        {
            switch (conversation)
            {
                case OpenAIConversation openAIConvo:
                    return MapOpenAIConvoToDto(openAIConvo);
                case OpenAIAssistantConversation openAIAssistantConversation:
                    return MapOpenAIAssistantConvoToDto(openAIAssistantConversation);
                case AssistantConversation assistantConvo:
                    return MapAssistantConvoToDto(assistantConvo);
                default:
                    return new ConversationDto
                    {
                        Id = conversation.Id,
                        Title = conversation.Title,
                        Model = conversation.Model,
                        CreatedAt = conversation.CreatedAt,
                        ConversationType = conversation.ConversationType,
                        Instructions = conversation.Instructions
                    };
            }
        }

        private OpenAIConversationDto MapOpenAIConvoToDto(OpenAIConversation conversation)
        {
            return new OpenAIConversationDto
            {
                Id = conversation.Id,
                Title = conversation.Title,
                Model = conversation.Model,
                CreatedAt = conversation.CreatedAt,
                ConversationType = conversation.ConversationType,
                Instructions = conversation.Instructions
            };
        }

        private OpenAIAssistantConversationDto MapOpenAIAssistantConvoToDto(OpenAIAssistantConversation conversation)
        {
            return new OpenAIAssistantConversationDto
            {
                Id = conversation.Id,
                Title = conversation.Title,
                Model = conversation.Model,
                CreatedAt = conversation.CreatedAt,
                ConversationType = conversation.ConversationType,
                AssistantType = conversation.AssistantType,
                AssistantId = conversation.AssistantId,
                ThreadId = conversation.ThreadId
            };
        }

        private AssistantConversationDto MapAssistantConvoToDto(AssistantConversation conversation)
        {
            return new AssistantConversationDto
            {
                Id = conversation.Id,
                Title = conversation.Title,
                Model = conversation.Model,
                CreatedAt = conversation.CreatedAt,
                ConversationType = conversation.ConversationType,
                AssistantType = conversation.AssistantType,
                AssistantId = conversation.AssistantId
            };
        }

        public Conversation MapToEntity(ConversationDto dto)
        {
            switch (dto)
            {
                case OpenAIConversationDto openAIConvoDto:
                    return MapOpenAIConvoDtoToEntity(openAIConvoDto);
                case OpenAIAssistantConversationDto openAIAssistantConversationDto:
                    return MapOpenAIAssistantConvoDtoToEntity(openAIAssistantConversationDto);
                case AssistantConversationDto assistantConvoDto:
                    return MapAssistantConvoDtoToEntity(assistantConvoDto);
                default:
                    return new Conversation
                    {
                        Id = dto.Id,
                        Title = dto.Title,
                        Model = dto.Model,
                        CreatedAt = dto.CreatedAt,
                        ConversationType = dto.ConversationType,
                        Instructions = dto.Instructions
                    };
            }
        }

        private Conversation MapAssistantConvoDtoToEntity(AssistantConversationDto assistantConvoDto)
        {
            return new AssistantConversation
            {
                Id = assistantConvoDto.Id,
                Title = assistantConvoDto.Title,
                Model = assistantConvoDto.Model,
                CreatedAt = assistantConvoDto.CreatedAt,
                ConversationType = assistantConvoDto.ConversationType,
                AssistantType = assistantConvoDto.AssistantType,
                AssistantId = assistantConvoDto.AssistantId
            };
        }

        private Conversation MapOpenAIAssistantConvoDtoToEntity(OpenAIAssistantConversationDto openAIAssistantConversationDto)
        {
            return new OpenAIAssistantConversation
            {
                Id = openAIAssistantConversationDto.Id,
                Title = openAIAssistantConversationDto.Title,
                Model = openAIAssistantConversationDto.Model,
                CreatedAt = openAIAssistantConversationDto.CreatedAt,
                ConversationType = openAIAssistantConversationDto.ConversationType,
                AssistantType = openAIAssistantConversationDto.AssistantType,
                AssistantId = openAIAssistantConversationDto.AssistantId,
                ThreadId = openAIAssistantConversationDto.ThreadId
            };
        }

        private Conversation MapOpenAIConvoDtoToEntity(OpenAIConversationDto openAIConvoDto)
        {
            return new OpenAIConversation
            {
                Id = openAIConvoDto.Id,
                Title = openAIConvoDto.Title,
                Model = openAIConvoDto.Model,
                CreatedAt = openAIConvoDto.CreatedAt,
                ConversationType = openAIConvoDto.ConversationType,
                Instructions = openAIConvoDto.Instructions
            };
        }

        public void UpdateEntityFromDto(ConversationDto dto, Conversation entity)
        {
            // TODO: Move out responsibility from DTO
            dto.UpdateEntity(entity);
        }
    }
}