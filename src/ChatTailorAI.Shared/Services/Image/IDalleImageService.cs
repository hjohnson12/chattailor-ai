using ChatTailorAI.Shared.Models.Image.OpenAI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Image
{
    public interface IDalleImageService
    {
        Task<IEnumerable<string>> GenerateImagesAsync(OpenAIImageGenerationRequest imageRequest);
        void UpdateAuthorizationHeader(string apiKey);
    }
}
