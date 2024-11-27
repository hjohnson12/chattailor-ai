using System.IO;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Audio
{
    public interface IAudioRecorderService
    {
        Task RecordAudio();
        Task<Stream> StopRecordingAudio();
        Task InitializeMediaCapture();
    }
}
