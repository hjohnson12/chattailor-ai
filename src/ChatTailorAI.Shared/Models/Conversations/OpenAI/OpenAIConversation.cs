namespace ChatTailorAI.Shared.Models.Conversations.OpenAI
{
    public class OpenAIConversation : Conversation
    {
        public OpenAIConversation() : base()
        {
            ConversationType = "OpenAI";
        }
    }
}