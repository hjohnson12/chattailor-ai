using ChatTailorAI.Shared.Dto.Chat;
using ChatTailorAI.Shared.Mappers.Interfaces;
using ChatTailorAI.Shared.Models.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatTailorAI.Shared.Mappers
{
    public class MessageMapper : IMessageMapper
    {
        private readonly IImageMapper _imageMapper;

        public MessageMapper(IImageMapper imageMapper)
        {
            _imageMapper = imageMapper;
        }

        public ChatMessageDto MapToDto(ChatMessage chatMessage)
        {
            switch (chatMessage)
            {
                case ChatImageMessage chatImageMessage:
                    return new ChatImageMessageDto(chatImageMessage);
                default:
                    return new ChatMessageDto(chatMessage);
            }
        }

        public ChatMessage MapToEntity(ChatMessageDto dto)
        {
            switch (dto)
            {
                case ChatImageMessageDto imageMessageDto:
                    return MapImageDtoToEntity(imageMessageDto);
                default:
                    return MapStandardDtoToEntity(dto);
            }
        }

        public void UpdateEntityFromDto(ChatMessageDto dto, ChatMessage entity)
        {
            entity.Id = dto.Id;
            entity.ConversationId = dto.ConversationId;
            entity.Role = dto.Role;
            entity.Content = dto.Content;
            entity.CreatedAt = dto.CreatedAt;
            entity.ExternalMessageId = dto.ExternalMessageId;

            if (dto is ChatImageMessageDto imageMessageDto)
            {
                var imageMessageEntity = entity as ChatImageMessage;
                if (imageMessageEntity != null)
                {
                    imageMessageEntity.Images = imageMessageDto.Images
                        .Select(i => _imageMapper.MapToEntity(i))
                        .ToList();
                }
            }
        }

        private ChatMessage MapStandardDtoToEntity(ChatMessageDto dto)
        {
            return new ChatMessage
            {
                Id = dto.Id,
                ConversationId = dto.ConversationId,
                Role = dto.Role,
                Content = dto.Content,
                CreatedAt = dto.CreatedAt,
                ExternalMessageId = dto.ExternalMessageId
            };
        }

        private ChatMessage MapImageDtoToEntity(ChatImageMessageDto dto)
        {
            return new ChatImageMessage
            {
                Id = dto.Id,
                ConversationId = dto.ConversationId,
                Role = dto.Role,
                Content = dto.Content,
                CreatedAt = dto.CreatedAt,
                ExternalMessageId = dto.ExternalMessageId,
                MessageType = dto.MessageType,
                Images = (dto.Images != null)
                    ? dto.Images.Select(i => _imageMapper.MapToEntity(i)).ToList()
                    : new List<ChatImage>()
            };
        }
    }
}
