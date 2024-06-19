using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatTailorAI.Shared.Models.Chat
{
    public class ChatImageMessage : ChatMessage
    {
        public virtual ICollection<ChatImage> Images { get; set; } = new List<ChatImage>();
        public ChatImage PrimaryImage => Images.FirstOrDefault();

        public ChatImageMessage()
        {
            Images = new List<ChatImage>();
        }

        public void AddImage(ChatImage image)
        {
            Images.Add(image);
        }
    }
}
