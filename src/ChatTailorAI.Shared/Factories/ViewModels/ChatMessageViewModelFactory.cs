using ChatTailorAI.Shared.Dto.Chat;
using ChatTailorAI.Shared.Factories.Interfaces;
using ChatTailorAI.Shared.Services.Files;
using ChatTailorAI.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Factories.ViewModels
{
    public class ChatMessageViewModelFactory : IChatMessageViewModelFactory
    {
        private readonly IImageFileService _imageFileService;

        public ChatMessageViewModelFactory(IImageFileService imageFileService)
        {
            _imageFileService = imageFileService;
        }

        public ChatMessageViewModel CreateViewModelFromDto(ChatMessageDto dto)
        {
            if (dto is ChatImageMessageDto imageDto)
            {
                return new ChatImageMessageViewModel(imageDto, _imageFileService);
            }
            else
            {
                return new ChatMessageViewModel(dto);
            }
        }

        public T CreateViewModel<T>(ChatMessageDto dto) where T : ChatMessageViewModel
        {
            if (typeof(T) == typeof(ChatImageMessageViewModel))
            {
                return new ChatImageMessageViewModel(dto as ChatImageMessageDto, _imageFileService) as T;
            }

            return new ChatMessageViewModel(dto) as T;
        }
    }
}