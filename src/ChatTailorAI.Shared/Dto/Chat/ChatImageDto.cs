using ChatTailorAI.Shared.Models.Chat;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Dto.Chat
{
    public class ChatImageDto
    {
        private string _url;

        public ChatImageDto()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
        }

        public ChatImageDto(ChatImage chatImage)
        {
            Id = chatImage.Id;
            Url = chatImage.Url;
            CreatedAt = chatImage.CreatedAt;
            ModelIdentifier = chatImage.ModelIdentifier;
            MessageId = chatImage.MessageId;
            PromptId = chatImage.PromptId;
            Size = chatImage.Size;
        }

        public string Id { get; set; }
        public string MessageId { get; set; }
        public string PromptId { get; set; }
        public string Url
        {
            get => _url;
            set
            {
                _url = value;
                LocalUri = new Uri($"ms-appdata:///local/{_url}");
            }
        }
        public string ModelIdentifier { get; set; }
        public string Size { get; set; }
        public DateTime CreatedAt { get; set; }

        // Property to bind to in the XAML
        public Uri LocalUri { get; private set; }
        public string Prompt { get; set; }
        public bool IsGeneratedImage => !string.IsNullOrEmpty(ModelIdentifier);
        public bool IsUserImage => string.IsNullOrEmpty(ModelIdentifier) && string.IsNullOrEmpty(PromptId);

        public static ChatImageDto FromChatImage(ChatImage chatImage)
        {
            return new ChatImageDto(chatImage);
        }
    }
}
