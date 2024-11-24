using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Models.Image;
using ChatTailorAI.Shared.Models.Image.OpenAI;

namespace ChatTailorAI.Shared.Events
{
    public class ImageGeneratedEvent
    {
        public PromptDto PromptDto { get; set; } 
        public ImageGenerationResponse<OpenAIImageGenerationSettings> ImageGenerationResponse { get; set; }
    }
}