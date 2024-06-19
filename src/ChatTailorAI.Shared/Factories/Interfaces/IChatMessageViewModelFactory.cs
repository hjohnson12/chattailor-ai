using ChatTailorAI.Shared.Dto.Chat;
using ChatTailorAI.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Factories.Interfaces
{
    public interface IChatMessageViewModelFactory
    {
        ChatMessageViewModel CreateViewModelFromDto(ChatMessageDto dto);
        T CreateViewModel<T>(ChatMessageDto dto) where T : ChatMessageViewModel;
    }
}
