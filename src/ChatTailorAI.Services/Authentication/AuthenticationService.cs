using System.Threading.Tasks;
using ChatTailorAI.Shared.Events;
using ChatTailorAI.Shared.Services.Authentication;
using ChatTailorAI.Shared.Services.Common;
using ChatTailorAI.Shared.Services.Events;

namespace ChatTailorAI.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ISpotifyService _spotifyService;

        private TaskCompletionSource<bool> _authCompletionSource;

        public AuthenticationService(
            IEventAggregator eventAggregator,
            ISpotifyService spotifyService)
        {
            _eventAggregator = eventAggregator;
            _spotifyService = spotifyService;
        }

        public Task<bool> BeginSpotifyAuthentication()
        {
            _authCompletionSource = new TaskCompletionSource<bool>();

            // Trigger the event that causes the UI to open the WebView.
            var authUrl = _spotifyService.GetAuthorizationUrl(new string[] { "playlist-read-private", "playlist-modify-public", "playlist-modify-private", "user-modify-playback-state" });
            _eventAggregator.PublishAuthenticationRequested(new AuthenticationRequestedEvent { AuthenticationUrl = authUrl });

            return _authCompletionSource.Task;
        }

        public void CompleteAuthentication(bool isSuccess)
        {
            _authCompletionSource.TrySetResult(isSuccess);

            // TODO: Figure out why its trying to SetResult multiple times sometimes
            // Noticed it after switching to settings and back, sending msg, and it trying to set again after completion
            // or getting GetUserPlaylists()
            if (!_authCompletionSource.Task.IsCompleted)
            {
                _authCompletionSource.SetResult(true);
            }
        }

        public void CancelAuthentication()
        {
            _authCompletionSource.SetCanceled();
        }

        public async Task<string> RequestSpotifyAccessToken(string authCode)
        {
            return await _spotifyService.RequestAccessToken(authCode);
        }

        public bool ValidateSpotifyAccessToken()
        {
            return _spotifyService.IsAccessTokenValid();
        }
    }
}