using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Models.Image.OpenAI;
using ChatTailorAI.Shared.Models.Image;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Image
{
    public interface IImageGenerationService
    {
        // TODO: Update to use generics once > 1 image generation service is supported (currently only OpenAI)
        Task<ImageGenerationResponse<OpenAIImageGenerationSettings>> GenerateImagesAsync(PromptDto imagePrompt);
    }
}
