using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Models.Chat.OpenAI.Content
{
    public interface IOpenAIChatContent
    {
        string Type { get; }
    }
}