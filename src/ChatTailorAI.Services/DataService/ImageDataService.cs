using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatTailorAI.Shared.Dto.Chat;
using ChatTailorAI.Shared.Mappers.Interfaces;
using ChatTailorAI.Shared.Repositories;
using ChatTailorAI.Shared.Services.DataServices;

namespace ChatTailorAI.Services.DataService
{
    public class ImageDataService : IImageDataService
    {
        private readonly IImageRepository _imageRepository;
        private readonly IImageMapper _imageMapper;

        public ImageDataService(
            IImageRepository imageRepository,
            IImageMapper imageMapper)
        {
            _imageRepository = imageRepository;
            _imageMapper = imageMapper;
        }

        public async Task DeleteImageAsync(ChatImageDto imageDto)
        {
            var image = _imageMapper.MapToEntity(imageDto);
            await _imageRepository.DeleteAsync(image);
        }

        public async Task DeleteImagesAsync(IEnumerable<ChatImageDto> imageDtos)
        {
            foreach (var imageDto in imageDtos)
            {
                var image = _imageMapper.MapToEntity(imageDto);
                await _imageRepository.DeleteAsync(image);
            }
        }

        public async Task<IEnumerable<ChatImageDto>> GetImagesAsync(string messageId)
        {
            var result = await _imageRepository.GetAllByMessageId(messageId);
            return result.Select(_imageMapper.MapToDto);
        }

        public async Task<IEnumerable<ChatImageDto>> GetImagesAsync()
        {
            var results = await _imageRepository.GetAllAsync();
            return results.Select(_imageMapper.MapToDto);
        }

        public async Task SaveImagesAsync(IEnumerable<ChatImageDto> imageDtos)
        {
            foreach (var imageDto in imageDtos)
            {
                var image = _imageMapper.MapToEntity(imageDto);
                await _imageRepository.AddAsync(image);
            }
        }
    }
}