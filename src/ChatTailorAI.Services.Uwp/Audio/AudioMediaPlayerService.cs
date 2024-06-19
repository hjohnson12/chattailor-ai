using ChatTailorAI.Shared.Services.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage.Streams;

namespace ChatTailorAI.Services.Uwp.Audio
{
    public class AudioMediaPlayerService : IAudioMediaPlayerService
    {
        public AudioMediaPlayerService() 
        { 
            MediaPlayer = new MediaPlayer();
        }

        ~AudioMediaPlayerService()
        {
            MediaPlayer?.Dispose();
        }

        private MediaPlayer MediaPlayer { get; set; }

        public async Task PlayAudio(Stream stream)
        {
            try
            {
                var mediaStreamSource = new InMemoryRandomAccessStream();
                await stream.CopyToAsync(mediaStreamSource.AsStreamForWrite());
                await mediaStreamSource.FlushAsync();

                mediaStreamSource.Seek(0);

                MediaPlayer.Source = MediaSource.CreateFromStream(mediaStreamSource, "audio/mpeg");  // "audio/mpeg" is the MIME type for MP3
                MediaPlayer.Play();

                // Need to dispose MediaPlayer later, store in a field or prop
            }
            catch
            {
                stream.Dispose();
                MediaPlayer.Dispose();
                throw;
            }
        }
    }
}
