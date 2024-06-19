using ChatTailorAI.Shared.Dto.Chat;
using ChatTailorAI.Shared.Mappers.Interfaces;
using ChatTailorAI.Shared.Services.Database.Repositories;
using ChatTailorAI.Shared.Services.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Services.DataService
{
    public class MessageDataService : IMessageDataService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMessageMapper _mapper;

        public MessageDataService(
            IMessageRepository messageRepository,
            IMessageMapper mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        public Task DeleteMessageAsync(ChatMessageDto chatMessageDto)
        {
            var entity = _mapper.MapToEntity(chatMessageDto);
            return _messageRepository.DeleteAsync(entity);
        }

        public async Task DeleteMessagesAsync()
        {
            await _messageRepository.DeleteAllAsync();
        }

        public async Task<IEnumerable<ChatMessageDto>> GetMessagesAsync(string conversationId)
        {
           var results = await _messageRepository.GetAllByConversationIdAsync(conversationId);
            return results.Select(_mapper.MapToDto);
        }

        public Task SaveMessageAsync(ChatMessageDto chatMessageDto)
        {
            var entity = _mapper.MapToEntity(chatMessageDto);
            return _messageRepository.AddAsync(entity);
        }

        public Task UpdateMessageAsync(ChatMessageDto chatMessageDto)
        {
            throw new NotImplementedException();
        }
    }
}