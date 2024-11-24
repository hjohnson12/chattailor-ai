namespace ChatTailorAI.Shared.Models.Image.OpenAI
{
    public class OpenAIImageGenerationRequest : ImageGenerationRequest<OpenAIImageGenerationSettings>
    {
        public OpenAIImageGenerationRequest() 
        { 
            // Default to DALL-E 3
            Model = "dall-e-3";
        }

        public OpenAIImageGenerationRequest(OpenAIImageGenerationSettings imageSettings)
        {
            Settings = imageSettings;
        }
    }
}