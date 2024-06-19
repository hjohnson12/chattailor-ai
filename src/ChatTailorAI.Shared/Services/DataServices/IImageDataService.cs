using ChatTailorAI.Shared.Dto.Chat;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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