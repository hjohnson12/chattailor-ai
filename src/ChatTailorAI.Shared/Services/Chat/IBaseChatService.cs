using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Services.Chat
{
    public interface IBaseChatService
    {
        bool ValidateApiKey();
    }
}
