using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<bool> BeginSpotifyAuthentication();
        Task<string> RequestSpotifyAccessToken(string authCode);
        void CompleteAuthentication(bool isSuccess);
        void CancelAuthentication();
        bool ValidateSpotifyAccessToken();
    }   
}