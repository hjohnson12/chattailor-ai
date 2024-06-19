using ChatTailorAI.Shared.Models.Chat;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Builders
{
    public interface IChatRequestBuilder
    {
        object BuildChatRequest(string model, string instructions, IEnumerable<object> messages, object settings);
    }

    //public interface IChatRequestBuilder
    //{
    //    ChatRequest BuildChatRequest(string model, string instructions, IEnumerable<IChatModelMessage> messages, ChatSettings settings);
    //}

    //public interface IChatRequestBuilder<TChatRequest, TMessage, TSettings>
    //    where TChatRequest : ChatRequest<TSettings, TMessage>
    //{
    //    TChatRequest BuildChatRequest(string model, string instructions, IEnumerable<TMessage> messages, TSettings settings);
    //}

    //public interface IChatRequestBuilder<TChatRequest, TMessage, TSettings>
    //    where TChatRequest : ChatRequest<TSettings, TMessage>
    //    where TMessage : IChatModelMessage
    //    where TSettings : ChatSettings
    //{
    //    TChatRequest BuildChatRequest(string model, string instructions, IEnumerable<TMessage> messages, TSettings settings);
    //}
}
