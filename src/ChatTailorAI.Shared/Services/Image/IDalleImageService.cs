using System.Collections.Generic;
using System.Threading.Tasks;
using ChatTailorAI.Shared.Models.Image.OpenAI;

namespace ChatTailorAI.Shared.Services.Image
{
    public interface IDalleImageService
    {
        Task<IEnumerable<string>> GenerateImagesAsync(OpenAIImageGenerationRequest imageRequest);
        void UpdateAuthorizationHeader(string apiKey);
    }
}
