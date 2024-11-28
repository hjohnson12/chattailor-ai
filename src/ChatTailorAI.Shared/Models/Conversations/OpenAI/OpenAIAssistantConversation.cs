namespace ChatTailorAI.Shared.Models.Conversations.OpenAI
{
    public class OpenAIAssistantConversation : AssistantConversation
    {
        public string ThreadId { get; set; }

        public OpenAIAssistantConversation() : base()
        {
            // AssistantId will be provided from the OpenAI API
            ConversationType = "OpenAI Assistant";
        }
    }
}