using System;
using System.Threading.Tasks;
using ChatTailorAI.Shared.Services.Common;
using ChatTailorAI.Shared.Services.Speech;

namespace ChatTailorAI.Services.Speech
{
    public class SpeechService : ISpeechService
    {
        private readonly IAzureSpeechService _azureSpeechService;
        private readonly IOpenAISpeechService _openAISpeechService;
        private readonly IElevenLabsSpeechService _elevenLabsSpeechService;
        private readonly IAudioMediaPlayerService _audioMediaPlayerService;
        private readonly IAppNotificationService _appNotificationService;

        public SpeechService(
            IAzureSpeechService azureSpeechService,
            IOpenAISpeechService openAISpeechService,
            IElevenLabsSpeechService elevenLabsSpeechService,
            IAudioMediaPlayerService audioMediaPlayerService,
            IAppNotificationService appNotificationService
            )
        {
            _azureSpeechService = azureSpeechService;
            _openAISpeechService = openAISpeechService;
            _elevenLabsSpeechService = elevenLabsSpeechService;
            _audioMediaPlayerService = audioMediaPlayerService;
            _appNotificationService = appNotificationService;
        }

        public async Task ProcessTextToSpeech(string speechProvider, string text)
        {
            try
            {
                switch (speechProvider)
                {
                    case "azure":
                        // Azure lib uses built in .NET audio player
                        await _azureSpeechService.SynthesizeSpeechAsync(text);
                        break;
                    case "openai":
                        var openAIStream = await _openAISpeechService.SynthesizeSpeechAsync(text);
                        await _audioMediaPlayerService.PlayAudio(openAIStream);
                        break;
                    case "elevenlabs":
                        var elevenLabsStream = await _elevenLabsSpeechService.SynthesizeSpeechAsync(text);
                        await _audioMediaPlayerService.PlayAudio(elevenLabsStream);
                        break;
                }
            }
            catch (Exception ex)
            {
                _appNotificationService.Display($"Failed to process text to speech: {ex.Message}");
            }
        }
    }
}
