using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Common
{
    public interface IAudioMediaPlayerService
    {
        Task PlayAudio(Stream stream);
    }
}
