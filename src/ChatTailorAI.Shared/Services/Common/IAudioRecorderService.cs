using System.IO;
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
