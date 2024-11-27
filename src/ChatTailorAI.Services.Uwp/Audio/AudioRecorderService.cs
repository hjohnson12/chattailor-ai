using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Media.Devices;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using ChatTailorAI.Shared.Services.Audio;

namespace ChatTailorAI.Services.Uwp.Audio
{
    public class AudioRecorderService : IAudioRecorderService
    {
        private InMemoryRandomAccessStream audioStream; 
        private MediaCapture mediaCapture;

        public async Task<bool> CheckMicrophonePermission()
        {
            try
            {
                var mediaCapture = new MediaCapture();
                var settings = new MediaCaptureInitializationSettings
                {
                    StreamingCaptureMode = StreamingCaptureMode.Audio
                };
                await mediaCapture.InitializeAsync(settings);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task InitializeMediaCapture()
        {
            try
            {
                mediaCapture = new MediaCapture();
                await mediaCapture.InitializeAsync(new MediaCaptureInitializationSettings
                {
                    MediaCategory = MediaCategory.Speech,
                    StreamingCaptureMode = StreamingCaptureMode.Audio
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task RecordAudio()
        {
            if (mediaCapture != null)
            {
                var audioInputDevices = await DeviceInformation.FindAllAsync(MediaDevice.GetAudioCaptureSelector());

                if (audioInputDevices.Count == 0)
                {
                    throw new Exception("No audio input devices found");
                }

                audioStream = new InMemoryRandomAccessStream();

                MediaEncodingProfile recordProfile = MediaEncodingProfile.CreateMp3(AudioEncodingQuality.High);
                try
                {
                    await mediaCapture.StartRecordToStreamAsync(recordProfile, audioStream);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred when trying to start recording: {ex.Message}");
                }
            }
        }

        public async Task<Stream> StopRecordingAudio()
        {
            await mediaCapture.StopRecordAsync();
            audioStream.Seek(0);

            return audioStream.AsStream();
        }
    }
}