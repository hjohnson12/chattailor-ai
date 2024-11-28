using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Common
{
    public interface IApplicationViewService
    {
        Task<bool> TryEnterPictureInPictureModeAsync();
    }
}