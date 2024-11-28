using System.Collections.Generic;
using System.Threading.Tasks;
using ChatTailorAI.Shared.Dto.Chat;

namespace ChatTailorAI.Shared.Services.DataServices
{
    public interface IImageDataService
    {
        Task SaveImagesAsync(IEnumerable<ChatImageDto> imageDtos);
        Task<IEnumerable<ChatImageDto>> GetImagesAsync();
        Task<IEnumerable<ChatImageDto>> GetImagesAsync(string messageId);
        Task DeleteImagesAsync(IEnumerable<ChatImageDto> imageDtos);
        Task DeleteImageAsync(ChatImageDto imageDto);
    }
}