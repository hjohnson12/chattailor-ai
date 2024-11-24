using System.Collections.Generic;

namespace ChatTailorAI.Shared.Builders
{
    /// <summary>
    /// Interface for chat request builders.
    /// </summary>
    public interface IChatRequestBuilder
    {
        /// <summary>
        /// Builds a chat request.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="instructions"></param>
        /// <param name="messages"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
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