using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Mappers.Interfaces;
using ChatTailorAI.Shared.Repositories;
using ChatTailorAI.Shared.Services.DataServices;

namespace ChatTailorAI.Services.DataService
{
    public class AssistantDataService : IAssistantDataService
    {
        private readonly IAssistantRepository _assistantRepository;
        private readonly IAssistantMapper _mapper;

        public AssistantDataService(
            IAssistantRepository assistantRepository,
            IAssistantMapper mapper)
        {
            _assistantRepository = assistantRepository;
            _mapper = mapper;
        }

        public async Task SaveAssistantAsync(AssistantDto assistant)
        {
            var entity = _mapper.MapToEntity(assistant);
            await _assistantRepository.AddAssistantAsync(entity);
        }

        public async Task<IEnumerable<AssistantDto>> GetAssistantsAsync()
        {
            var results = await _assistantRepository.GetAllAsync();
            return results.Select(_mapper.MapToDto);
        }

        public async Task DeleteAssistantAsync(AssistantDto assistantDto)
        {
            var assistantToUpdate = await _assistantRepository.GetAsync(assistantDto.Id);
            if (assistantToUpdate != null)
            {
                // Note: Temp fix for EF Core tracking issue
                _mapper.UpdateEntityFromDto(assistantDto, assistantToUpdate);
                await _assistantRepository.DeleteAsync(assistantToUpdate);
            }
        }

        public async Task DeleteAssistantsAsync()
        {
            await _assistantRepository.DeleteAllAsync();
        }

        public async Task<AssistantDto> GetAssistantAsync(AssistantDto assistant)
        {
            var result = await _assistantRepository.GetAsync(assistant.Id);
            return _mapper.MapToDto(result);
        }

        public async Task UpdateAssistantAsync(AssistantDto assistantDto)
        {
            //await _assistantRepository.UpdateAsync(MapToEntity(assistant));
            var assistantToUpdate = await _assistantRepository.GetAsync(assistantDto.Id);
            if (assistantToUpdate != null)
            {
                // Map properties from DTO to entity
                // Note: Temp fix for EF Core tracking issue
                _mapper.UpdateEntityFromDto(assistantDto, assistantToUpdate);
                await _assistantRepository.UpdateAsync(assistantToUpdate);
            }
        }

        public async Task<AssistantDto> GetAssistantByIdAsync(string id)
        {
            var result = await _assistantRepository.GetAsync(id);
            if (result == null)
                return null;

            return _mapper.MapToDto(result);
        }

        public async Task<Dictionary<string, string>> GetAssistantNamesByIdsAsync(IEnumerable<string> ids)
        {
            var results = await _assistantRepository.GetByIdsAsync(ids);
            var assistantNames = results.ToDictionary(assistant => assistant.Id, assistant => assistant.Name);

            return assistantNames;
        }
    }
}