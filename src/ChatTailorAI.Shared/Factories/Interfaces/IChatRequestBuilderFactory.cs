using ChatTailorAI.Shared.Builders;

namespace ChatTailorAI.Shared.Factories.Interfaces
{
    /// <summary>
    /// Factory for creating chat request builders.
    /// </summary>
    public interface IChatRequestBuilderFactory
    {
        /// <summary>
        /// Creates a chat request builder for the given chat service.
        /// </summary>
        /// <param name="chatServiceName"></param>
        /// <returns></returns>
        IChatRequestBuilder GetBuilder(string chatServiceName);
    }
}