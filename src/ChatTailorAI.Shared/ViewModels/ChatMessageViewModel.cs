using ChatTailorAI.Shared.Base;
using ChatTailorAI.Shared.Dto.Chat;
using ChatTailorAI.Shared.Dto.Chat.OpenAI;
using ChatTailorAI.Shared.Models.Chat;
using ChatTailorAI.Shared.Models.Chat.OpenAI;
using ChatTailorAI.Shared.Services.Files;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.ViewModels
{
    public class ChatMessageViewModel : Observable
    {
        private string _id;
        private string _conversationId;
        private string _role;
        private string _content;
        private DateTime _createdAt;
        private string _externalMessageId;
        private MessageType _messageType;

        // Properties specific to certain message types
        // Possibly move to a subclass later on
        private string _errorMessage;

        public ChatMessageViewModel()
        {
            _id = Guid.NewGuid().ToString();
            _createdAt = DateTime.Now;
        }

        public ChatMessageViewModel(ChatMessageDto dto)
        {
            _id = dto.Id;
            _conversationId = dto.ConversationId;
            _role = dto.Role;
            _content = dto.Content;
            _createdAt = dto.CreatedAt;
            _externalMessageId = dto.ExternalMessageId;
            _messageType = dto.MessageType;

            //_messageType = dto is ChatImageMessageDto ? MessageType.Image : MessageType.Text;
        }

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string ConversationId
        {
            get => _conversationId;
            set => SetProperty(ref _conversationId, value);
        }

        public string Role
        {
            get => _role;
            set => SetProperty(ref _role, value);
        }

        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set => SetProperty(ref _createdAt, value);
        }

        public string ExternalMessageId
        {
            get => _externalMessageId;
            set => SetProperty(ref _externalMessageId, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public MessageType MessageType
        {
            get => _messageType;
            set => SetProperty(ref _messageType, value);
        }

        [JsonIgnore]
        public bool IsUser => Role == "user";

        // TODO: Change to use MessageType enum
        [JsonIgnore]
        public virtual bool IsImageMessage => this is ChatImageMessageViewModel;
        //public bool IsImageMessage => _messageType == MessageType.Image;

        [JsonIgnore]
        public bool IsErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        public void UpdateFromDto(ChatMessageDto dto)
        {
            _id = dto.Id;
            _conversationId = dto.ConversationId;
            _role = dto.Role;
            _content = dto.Content;
            _createdAt = dto.CreatedAt;
            _externalMessageId = dto.ExternalMessageId;
            _errorMessage = dto.ErrorMessage;
        }

        public void UpdateFromDto(OpenAIChatResponseDto dto)
        {
            _role = dto.Role;
            _content = dto.Content;
        }
    }
}