using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Mappers.Interfaces;
using ChatTailorAI.Shared.Repositories;
using ChatTailorAI.Shared.Services.DataServices;

namespace ChatTailorAI.Services.DataServices
{
    public class PromptDataService : IPromptDataService
    {
        private readonly IPromptRepository _promptRepository;
        private readonly IPromptMapper _promptMapper;

        public PromptDataService(
            IPromptRepository promptRepository,
            IPromptMapper promptMapper
            )
        {
            _promptRepository = promptRepository;
            _promptMapper = promptMapper;
        }

        public async Task AddPromptAsync(PromptDto prompt)
        {
            var entity = _promptMapper.MapToEntity(prompt);
            await _promptRepository.AddPromptAsync(entity);
        }

        public async Task DeleteAllAsync()
        {
            await _promptRepository.DeleteAllAsync();
        }

        public async Task DeleteAsync(PromptDto chatPromptDto)
        {
            var promptToDelete = await _promptRepository.GetAsync(chatPromptDto.Id.ToString());
            if (promptToDelete != null)
            {
                // Map properties from DTO to entity
                // Note: Temp fix for EF Core tracking issue
                _promptMapper.UpdateEntityFromDto(chatPromptDto, promptToDelete);
                await _promptRepository.DeleteAsync(promptToDelete);
            }
        }

        public async Task<IEnumerable<PromptDto>> GetAllAsync()
        {
            var results = await _promptRepository.GetAllAsync();
            return results.Select(_promptMapper.MapToDto);
        }

        public async Task<PromptDto> GetAsync(string id)
        {
            var prompt = await _promptRepository.GetAsync(id);
            if (prompt != null)
            {
                return _promptMapper.MapToDto(prompt);
            }

            return null;
        }

        public async Task UpdateAsync(PromptDto prompt)
        {
            var promptToUpdate = await _promptRepository.GetAsync(prompt.Id.ToString());
            if (promptToUpdate != null)
            {
                // Map properties from DTO to entity
                // Note: Temp fix for EF Core tracking issue
                _promptMapper.UpdateEntityFromDto(prompt, promptToUpdate);
                await _promptRepository.UpdateAsync(promptToUpdate);
            }
        }
    }
}
