using ChatTailorAI.Shared.Dto.Chat;
using ChatTailorAI.Shared.Dto.Chat.OpenAI;
using ChatTailorAI.Shared.Models.Assistants.OpenAI;
using ChatTailorAI.Shared.Services.Files;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ChatTailorAI.Shared.ViewModels
{
    public class ChatImageMessageViewModel : ChatMessageViewModel
    {
        private readonly IImageFileService _imageFileService;
        private List<ChatImageDto> _images;

        public List<ChatImageDto> Images
        {
            get => _images;
            set => SetProperty(ref _images, value);
        }

        public ChatImageMessageViewModel(ChatImageMessageDto dto) : base(dto)
        {
            Images = dto.Images;
        }

        public ChatImageMessageViewModel(ChatImageMessageDto dto, IImageFileService imageFileService) : base(dto)
        {
            Images = dto.Images;
            _imageFileService = imageFileService;
        }

        [JsonIgnore]
        public override bool IsImageMessage => true;

        [JsonIgnore]
        public bool IsImagesEmpty => Images == null || Images.Count == 0;

        [JsonIgnore]
        public bool IsTypingMessage => Content == "Assistant is generating image...";
    }
}
