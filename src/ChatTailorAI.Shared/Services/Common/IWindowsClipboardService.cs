using ChatTailorAI.Shared.Dto.Chat;
using ChatTailorAI.Shared.Models.Chat.OpenAI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Common
{
    public interface IWindowsClipboardService
    {
        void CopyToClipboard(ChatMessageDto message);
        Task<byte[]> GetImageFromClipboardAsync();
        Task<byte[]> GetImageFromClipboardAsPngAsync();
    }
}
