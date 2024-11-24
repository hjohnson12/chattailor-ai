using System.IO;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Speech
{
    public interface IWhisperService
    {
        Task<string> Transcribe(string filename, byte[] audioBuffer);
        Task<string> Translate(string filename, byte[] audioBuffer);
        Task<byte[]> StreamToBuffer(Stream stream);
    }
}
