using ChatTailorAI.Shared.Dto.Chat;
using ChatTailorAI.Shared.Models.Chat;

namespace ChatTailorAI.Shared.Mappers.Interfaces
{
    public interface IImageMapper
    {
        ChatImageDto MapToDto(ChatImage assistant);
        ChatImage MapToEntity(ChatImageDto dto);
        void UpdateEntityFromDto(ChatImageDto dto, ChatImage entity);
    }
}