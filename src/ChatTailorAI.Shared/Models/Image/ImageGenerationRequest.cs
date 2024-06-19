using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Models.Image
{
    public class ImageGenerationRequest<TSettings>
    {
        public string Model { get; set; }
        public string Prompt { get; set; }
        public TSettings Settings { get; set; }
    }
}
