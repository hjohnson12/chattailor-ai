using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Speech
{
    public interface IAzureSpeechService
    {
        Task SynthesizeSpeechAsync(string text);
        Task<List<string>> GetVoicesListAsync();
        Task<List<string>> GetModelsListAsync();
    }
}