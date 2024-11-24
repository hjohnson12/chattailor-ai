using System.Collections.Generic;
using System.Threading.Tasks;
using ChatTailorAI.Shared.Models.Audio;

namespace ChatTailorAI.Shared.Services.Common
{
    public interface ISpotifyService
    {
        Task<string> RefreshAccessToken();
        string GetAuthorizationUrl(string[] scopes);
        // Not working in UWP
        //Task AuthenticateWithBroker();
        Task<string> RequestAccessToken(string authCode);
        Task<SpotifyPlaylist> GetUserPlaylists();
        Task<List<dynamic>> GetPlaylistTracks(string playlistId);
        Task<string> GetPlaylistIdByName(string playlistName);
        bool IsAccessTokenValid();
        Task<string> SearchTrack(string name, string artist);
        Task<string> SearchAndAddTrack(string playlistName, string name, string artist);
        Task<string> AddTrackToPlaylist(string playlistId, string trackUri);
        Task<string> SearchAndPlayTrack(string name, string artist);
    }
}
