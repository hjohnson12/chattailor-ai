using ChatTailorAI.Shared.Dto.Chat.OpenAI;
using ChatTailorAI.Shared.Models.Chat.OpenAI;

namespace ChatTailorAI.Shared.Mappers.Interfaces
{
    public interface IOpenAIChatRequestMapper
    {
        OpenAIChatRequestDto MapToDto(OpenAIChatRequest request);
    }
}