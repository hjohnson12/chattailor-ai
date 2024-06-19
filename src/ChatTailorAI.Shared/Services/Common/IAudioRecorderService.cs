using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Common
{
    public interface IAudioRecorderService
    {
        Task RecordAudio();
        Task<Stream> StopRecordingAudio();
        Task InitializeMediaCapture();
    }
}
