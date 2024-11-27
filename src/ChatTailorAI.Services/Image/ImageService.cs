using System.Threading.Tasks;
using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Models.Image.OpenAI;
using ChatTailorAI.Shared.Models.Image;
using ChatTailorAI.Shared.Services.Image;
using ChatTailorAI.Shared.Models.Settings;
using ChatTailorAI.Shared.Services.Common;

namespace ChatTailorAI.Services.Image
{
    public class ImageService : IImageService
    {
        private readonly IImageGenerationService _generationService;
        private readonly IUserSettingsService _userSettingsService;

        public ImageService(
            IImageGenerationService generationService,
            IUserSettingsService userSettingsService)
        {
            _generationService = generationService;
            _userSettingsService = userSettingsService;
        }

        public async Task<ImageGenerationResponse<OpenAIImageGenerationSettings>> GenerateImagesAsync(PromptDto imagePrompt)
        {
            // TODO: Update method to work with multiple image generation services (Dalle, StabilityAI, etc.)
            var openAIImageRequest = CreateImageRequest(imagePrompt);
            var imageUrls = await _generationService.GenerateImagesAsync(openAIImageRequest);

            return new ImageGenerationResponse<OpenAIImageGenerationSettings>
            {
                ImageUrls = imageUrls,
                Settings = openAIImageRequest.Settings
            };
        }

        private OpenAIImageGenerationRequest CreateImageRequest(PromptDto imagePrompt)
        {
            var imageModel = _userSettingsService.Get<string>(UserSettings.ImageModel);
            var numberOfImages = imageModel.Equals("dall-e-3")
            ? 1 // Only 1 supported at a time with Dalle3
                : int.Parse(_userSettingsService.Get<string>(UserSettings.ImageCount));
            var imageQuality = imageModel.Equals("dall-e-3")
                ? _userSettingsService.Get<string>(UserSettings.ImageQuality)
                : null;

            var openAIImageRequest = new OpenAIImageGenerationRequest
            {
                Model = imageModel,
                Prompt = imagePrompt.Content,
                Settings = new OpenAIImageGenerationSettings
                {
                    Model = imageModel,
                    Prompt = imagePrompt.Content,
                    N = numberOfImages,
                    // The size of the generated images
                    // Must be one of 256x256, 512x512, or 1024x1024 for dall-e-2
                    // Must be one of 1024x1024, 1792x1024, or 1024x1792 for dall-e-3 models
                    // TODO: Update this to be configurable, but for now, just use 1024x1024
                    Size = "1024x1024",
                    ImageQuality = imageQuality
                }
            };

            return openAIImageRequest;
        }
    }
}
