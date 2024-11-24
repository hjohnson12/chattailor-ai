using ChatTailorAI.Shared.Transformers;

namespace ChatTailorAI.Shared.Factories.Interfaces
{
    /// <summary>
    /// Factory for creating chat message transformers.
    /// </summary>
    public interface IChatMessageTransformerFactory
    {
        /// <summary>
        /// Creates a chat message transformer for the given chat service.
        /// </summary>
        /// <param name="chatServiceName"></param>
        /// <returns></returns>
        IChatMessageTransformer Create(string chatServiceName);
    }
}
