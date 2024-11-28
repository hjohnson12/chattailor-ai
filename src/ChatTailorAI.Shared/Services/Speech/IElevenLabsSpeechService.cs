using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Speech
{
    public interface IElevenLabsSpeechService
    {
        Task<Stream> SynthesizeSpeechAsync(string text);
        Task<List<dynamic>> GetVoicesListAsync();
        Task<List<string>> GetVoiceNamesAsync();
        Task<string> GetVoiceIdAsync(string voiceName);
        Task<List<string>> GetModelsListAsync();
    }
}
