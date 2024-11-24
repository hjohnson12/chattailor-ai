using System.Collections.Generic;

namespace ChatTailorAI.Shared.Models.Image
{
    public class ImageGenerationResponse<TSettings>
    {
        public IEnumerable<string> ImageUrls { get; set; }
        public TSettings Settings { get; set; }
    }
}