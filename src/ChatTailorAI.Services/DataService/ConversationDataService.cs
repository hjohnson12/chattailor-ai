using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Dto.Conversations;
using ChatTailorAI.Shared.Mappers.Interfaces;
using ChatTailorAI.Shared.Models.Assistants;
using ChatTailorAI.Shared.Models.Conversations;
using ChatTailorAI.Shared.Services.Database.Repositories;
using ChatTailorAI.Shared.Services.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Services.DataService
{
    public class ConversationDataService : IConversationDataService
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IConversationMapper _mapper;

        public ConversationDataService(
            IConversationRepository conversationRepository,
            IConversationMapper mapper)
        {
            _conversationRepository = conversationRepository;
            _mapper = mapper;
        }

        public async Task SaveConversationAsync(ConversationDto conversation)
        {
            var entity = _mapper.MapToEntity(conversation);
            await _conversationRepository.AddConversationAsync(entity);
        }

        public async Task<IEnumerable<ConversationDto>> GetConversationsAsync()
        {
            var results = await _conversationRepository.GetAllAsync();
            return results.Select(_mapper.MapToDto);
        }

        public async Task DeleteConversationAsync(ConversationDto conversation)
        {
            var entity = _mapper.MapToEntity(conversation);
            await _conversationRepository.DeleteAsync(entity.Id);
        }

        public async Task UpdateConversationAsync(ConversationDto conversationDto)
        {
            var conversationToUpdate = await _conversationRepository.GetAsync(conversationDto.Id);
            if (conversationToUpdate != null)
            {
                // Map properties from DTO to entity
                // Note: Temp fix for EF Core tracking issue (updating directly instead of instantiate)
                _mapper.UpdateEntityFromDto(conversationDto, conversationToUpdate);
                await _conversationRepository.UpdateAsync(conversationToUpdate);
            }
        }
    }
}