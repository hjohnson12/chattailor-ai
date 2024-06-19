using ChatTailorAI.Shared.Dto.Chat.OpenAI;
using ChatTailorAI.Shared.Dto.Chat;
using ChatTailorAI.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Mappers.Interfaces
{
    public interface IChatMessageViewModelMapper
    {
        ChatMessageDto MapToDto(ChatMessageViewModel viewModel);
        ChatMessageViewModel MapToViewModel(ChatMessageDto dto);
        void UpdateViewModelFromDto(ChatMessageDto dto, ChatMessageViewModel viewModel);
        void UpdateViewModelFromDto(OpenAIChatResponseDto dto, ChatMessageViewModel viewModel);
    }
}