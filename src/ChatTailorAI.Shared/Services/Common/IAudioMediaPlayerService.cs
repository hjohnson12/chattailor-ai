using System.IO;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Common
{
    public interface IAudioMediaPlayerService
    {
        Task PlayAudio(Stream stream);
    }
}
