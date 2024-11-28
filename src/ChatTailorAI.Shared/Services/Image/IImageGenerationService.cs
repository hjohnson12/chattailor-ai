using System.Threading.Tasks;
using System.Collections.Generic;
using ChatTailorAI.Shared.Models.Image.OpenAI;

namespace ChatTailorAI.Shared.Services.Image
{
    public interface IImageGenerationService
    {
        // TODO: Update to use generics once > 1 image generation service is supported (currently only OpenAI)
        Task<IEnumerable<string>> GenerateImagesAsync(OpenAIImageGenerationRequest imageRequest);
        void UpdateAuthorizationHeader(string apiKey);
    }
}