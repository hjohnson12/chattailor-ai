using System.IO;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Audio
{
    public interface IAudioMediaPlayerService
    {
        Task PlayAudio(Stream stream);
    }
}
