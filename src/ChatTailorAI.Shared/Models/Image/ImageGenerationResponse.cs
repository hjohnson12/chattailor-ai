using ChatTailorAI.Shared.Models.Image.OpenAI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Models.Image
{
    public class ImageGenerationResponse<TSettings>
    {
        public IEnumerable<string> ImageUrls { get; set; }
        public TSettings Settings { get; set; }
    }
}
