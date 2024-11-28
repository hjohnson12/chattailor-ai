using System;
using ChatTailorAI.Shared.Dto.Chat;
using ChatTailorAI.Shared.Dto.Chat.OpenAI;
using ChatTailorAI.Shared.Mappers.Interfaces;
using ChatTailorAI.Shared.ViewModels;

namespace ChatTailorAI.Shared.Mappers.ViewModels
{
    public class ChatMessageViewModelMapper : IChatMessageViewModelMapper
    {
        public ChatMessageDto MapToDto(ChatMessageViewModel viewModel)
        {
            switch (viewModel)
            {
                case ChatImageMessageViewModel imageMessageViewModel:
                    return MapToChatImageMessageDto(imageMessageViewModel);
                case ChatMessageViewModel messageViewModel:
                    return MapToChatMessageDto(messageViewModel);
                default:
                    return MapToChatMessageDto(viewModel);
            }
        }

        private ChatImageMessageDto MapToChatImageMessageDto(ChatImageMessageViewModel viewModel)
        {
            return new ChatImageMessageDto
            {
                Id = viewModel.Id,
                ConversationId = viewModel.ConversationId,
                Role = viewModel.Role,
                Content = viewModel.Content,
                CreatedAt = viewModel.CreatedAt,
                ExternalMessageId = viewModel.ExternalMessageId,
                MessageType = viewModel.MessageType,
                Images = viewModel.Images
            };
        }

        private ChatMessageDto MapToChatMessageDto(ChatMessageViewModel viewModel)
        {
            return new ChatMessageDto
            {
                Id = viewModel.Id,
                ConversationId = viewModel.ConversationId,
                Role = viewModel.Role,
                Content = viewModel.Content,
                CreatedAt = viewModel.CreatedAt,
                ExternalMessageId = viewModel.ExternalMessageId,
                MessageType = viewModel.MessageType,
                // Need in dtos? 
                ErrorMessage = viewModel.ErrorMessage
            };
        }

        private OpenAIBaseChatMessageDto MapToOpenAIDto(ChatMessageViewModel viewModel)
        {
            // Possibly inject IImageFileService _imageFileService = null
            var role = viewModel.Role;
            switch (role)
            {
                case "user":
                    // TODO: Map string content and image content
                    // to request for API call
                    return new OpenAIUserChatMessageDto
                    {
                        Role = viewModel.Role,
                        Content = viewModel.Content,
                    };
                //return Task.FromResult(new OpenAIUserChatMessageDto
                //{
                //    Role = Role,
                //    Content = Content,
                //});
                case "assistant":
                    return new OpenAIAssistantChatMessageDto
                    {
                        Role = viewModel.Role,
                        Content = viewModel.Content,
                        // Set Name, ToolCalls, and FunctionCall properties if available ?
                        // Function call comes after initial response typically
                    };
                default:
                    throw new InvalidOperationException("Unknown message role");
            }
        }

        public ChatMessageViewModel MapToViewModel(ChatMessageDto dto)
        {
            throw new NotImplementedException();
        }

        public void UpdateViewModelFromDto(ChatMessageDto dto, ChatMessageViewModel viewModel)
        {
            viewModel.Id = dto.Id;
            viewModel.ConversationId = dto.ConversationId;
            viewModel.Role = dto.Role;
            viewModel.Content = dto.Content;
            viewModel.CreatedAt = dto.CreatedAt;
            viewModel.ExternalMessageId = dto.ExternalMessageId;
            viewModel.ErrorMessage = dto.ErrorMessage;
        }

        public void UpdateViewModelFromDto(OpenAIChatResponseDto dto, ChatMessageViewModel viewModel)
        {
            viewModel.Role = dto.Role;
            viewModel.Content = dto.Content;
        }
    }
}