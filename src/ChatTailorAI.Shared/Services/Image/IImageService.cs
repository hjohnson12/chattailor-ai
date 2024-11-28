using System.Threading.Tasks;
using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Models.Image.OpenAI;
using ChatTailorAI.Shared.Models.Image;

namespace ChatTailorAI.Shared.Services.Image
{
    public interface IImageService
    {
        Task<ImageGenerationResponse<OpenAIImageGenerationSettings>> GenerateImagesAsync(PromptDto imagePrompt);
    }
}