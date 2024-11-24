using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Speech
{
    public interface ISpeechService
    {
        Task ProcessTextToSpeech(string speechProvider, string text);
    }
}
