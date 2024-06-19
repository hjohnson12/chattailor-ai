using ChatTailorAI.Shared.Dto.Chat;
using ChatTailorAI.Shared.Mappers.Interfaces;
using ChatTailorAI.Shared.Models.Chat;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Mappers
{
    public class ImageMapper : IImageMapper
    {
        public void UpdateEntityFromDto(ChatImageDto dto, ChatImage entity)
        {
            entity.Id = dto.Id;
            entity.Url = dto.Url;
            entity.CreatedAt = dto.CreatedAt;
            entity.MessageId = dto.MessageId;
            entity.Size = dto.Size;
            entity.ModelIdentifier = dto.ModelIdentifier;
            entity.PromptId = dto.PromptId;
        }

        public ChatImageDto MapToDto(ChatImage chatImage)
        {
            return new ChatImageDto
            {
                Id = chatImage.Id,
                PromptId = chatImage.PromptId,
                Url = chatImage.Url,
                CreatedAt = chatImage.CreatedAt,
                MessageId = chatImage.MessageId,
                Size = chatImage.Size,
                ModelIdentifier = chatImage.ModelIdentifier,
                Prompt = chatImage.Prompt?.Content
            };
        }

        public ChatImage MapToEntity(ChatImageDto dto)
        {
            return new ChatImage
            {
                Id = dto.Id,
                MessageId = dto.MessageId,
                PromptId = dto.PromptId,
                Url = dto.Url,
                ModelIdentifier = dto.ModelIdentifier,
                Size = dto.Size,
                CreatedAt = dto.CreatedAt,
            };
        }
    }
}