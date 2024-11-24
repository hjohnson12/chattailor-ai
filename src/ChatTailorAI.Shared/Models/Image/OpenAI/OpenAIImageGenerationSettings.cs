namespace ChatTailorAI.Shared.Models.Image.OpenAI
{
    public class OpenAIImageGenerationSettings : ImageGenerationSettings
    {
        public string Prompt { get; set; }
        public int N { get; set; }
        public string Size { get; set; }

        public string ImageQuality { get; set; }

        public OpenAIImageGenerationSettings()
        {
            // Set as default image model
            Model = "dall-e-3";
        }
    }
}