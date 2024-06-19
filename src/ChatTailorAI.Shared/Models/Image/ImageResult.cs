using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Models.Image
{
    /// <summary>
    /// Represents an image saved to the application folder
    /// </summary>
    public class ImageResult
    {
        public string Url { get; set; }
        public byte[] Bytes { get; set; }
        public string RelativePath { get; set; }
    }
}
