using ChatTailorAI.Shared.Models.Audio;
using ChatTailorAI.Shared.Services.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
//using Windows.Security.Authentication.Web; // Broker not working in UWP

namespace ChatTailorAI.Services.Tools
{
    public class SpotifyService : ISpotifyService
    {
        private readonly HttpClient _httpClient;
        private readonly IAppSettingsService _appSettingsService;
        private readonly string _redirectUri;

        public SpotifyService(
            IAppSettingsService appSettingsService,
            HttpClient client)
        {
            ClientId = appSettingsService.SpotifyClientId;
            ClientSecret = appSettingsService.SpotifyClientSecret;
            
            RefreshToken = "";
            AccessToken = "";
            _redirectUri = "http://localhost:5000/callback";

            var spotifyApiKey = appSettingsService.SpotifyApiKey;

            client.BaseAddress = new Uri("https://api.spotify.com/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {spotifyApiKey}");
            _httpClient = client;
        }

        private string ClientId { get; }
        private string ClientSecret { get; }
        private string RefreshToken { get; set; }
        private string AccessToken { get; set; }

        public async Task<string> RefreshAccessToken()
        {
            var tokenUrl = "https://accounts.spotify.com/api/token";
            var encodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ClientId}:{ClientSecret}"));
            var paramsContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", RefreshToken)
            });

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedCredentials);
            var response = await _httpClient.PostAsync(tokenUrl, paramsContent);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<dynamic>(responseContent);

            return data.access_token;
        }

        public string GetAuthorizationUrl(string[] scopes)
        {
            var authUrl = new UriBuilder("https://accounts.spotify.com/authorize");
            var queryParams = HttpUtility.ParseQueryString(authUrl.Query);
            queryParams["client_id"] = ClientId;
            queryParams["response_type"] = "code";
            queryParams["redirect_uri"] = _redirectUri;
            queryParams["scope"] = string.Join("%20", scopes);
            authUrl.Query = queryParams.ToString();

            return authUrl.ToString();
        }

        //public async Task AuthenticateWithBroker()
        //{
        //    // TODO: Not currently used / working, popup is blank on spotify urls
        //    var scopes = new string[] { "playlist-read-private", "playlist-modify-public", "playlist-modify-private", "user-modify-playback-state" };
        //    var authUrl = GetAuthorizationUrl(scopes);
        //    Uri requestUri = new Uri(authUrl);
        //    Uri callbackUri = new Uri("http://localhost:5000/callback");
        //    string sid = WebAuthenticationBroker.GetCurrentApplicationCallbackUri().Host.ToUpper();
        //    //Uri callbackUri = new Uri($"ms-app://{sid}/");
        //    try
        //    {
        //        WebAuthenticationResult result = await WebAuthenticationBroker.AuthenticateAsync(
        //                                                 WebAuthenticationOptions.None,
        //                                                 requestUri,
        //                                                 callbackUri);
        //        if (result.ResponseStatus == WebAuthenticationStatus.Success)
        //        {
        //            Uri responseData = new Uri(result.ResponseData);
        //            var authCode = HttpUtility.ParseQueryString(responseData.Query).Get("code");

        //            // Use the authorization code to get an access token from Spotify
        //            var accessToken = await RequestAccessToken(authCode);
        //            AccessToken = accessToken;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log or handle the exception
        //        Debug.WriteLine(ex.ToString());
        //    }
        //}

        public async Task<string> RequestAccessToken(string authCode)
        {
            var tokenUrl = "https://accounts.spotify.com/api/token";
            var encodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ClientId}:{ClientSecret}"));
            var paramsContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", authCode),
                new KeyValuePair<string, string>("redirect_uri", _redirectUri)
            });

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedCredentials);
            var response = await _httpClient.PostAsync(tokenUrl, paramsContent);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<dynamic>(responseContent);

            RefreshToken = data.refresh_token;
            AccessToken = data.access_token;
            return AccessToken;
        }

        public async Task<SpotifyPlaylist> GetUserPlaylists()
        {
            var endpoint = "v1/me/playlists?limit=50";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.AccessToken);

            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<SpotifyPlaylist>(responseContent);

            return data;
        }

        public async Task<string> GetPlaylistIdByName(string playlistName)
        {
            var playlists = await GetUserPlaylists();
            foreach (var playlist in playlists.Items)
            {
                if (playlist.Name.ToString().ToLower() == playlistName.ToLower())
                {
                    return playlist.Id.ToString();
                }
            }

            return null;
        }

        public async Task<List<dynamic>> GetPlaylistTracks(string playlistId)
        {
            var endpoint = $"v1/playlists/{playlistId}/tracks";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.AccessToken);

            var response = await _httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<dynamic>(responseContent);

                var tracks = new List<dynamic>();
                foreach (var item in data.items)
                {
                    tracks.Add(new { artist = item.track.artists[0].name, title = item.track.name });
                }

                return tracks;
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                var errMsg = "No tracks found for that playlist. The playlist may not have been located, please try again or reword the prompt.";
                throw new Exception(errMsg);
            }
            else
            {
                throw new Exception($"Failed to get playlist tracks. Status code {response.StatusCode}");
            }
        }

        public bool IsAccessTokenValid()
        {
            // TODO: Validate if access token is still valid? ]
            return AccessToken != null && AccessToken != string.Empty;
        }

        public async Task PlayTrack(string uri)
        {
            var endpoint = "v1/me/player/play";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.AccessToken);
            var content = new StringContent(JsonConvert.SerializeObject(new { uris = new[] { uri } }), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(endpoint, content);
            if (response.IsSuccessStatusCode)
            {
                return;
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new Exception("Failed to play song, spotify isn't currently active or recently used on a device.");
            }
        }

        public async Task<string> SearchAndPlayTrack(string name, string artist)
        {
            this.AccessToken = await this.RefreshAccessToken();

            var uri = await this.SearchTrack(name, artist);
            if (uri != null)
            {
                await this.PlayTrack(uri);
                return $"Successfully started playing {name} by {artist}";
            }
            else
            {
                return $"Track {name} by {artist} not found";
            }
        }

        public async Task<string> SearchTrack(string name, string artist)
        {
            var endpoint = $"v1/search?q=track:{name}%20artist:{artist}&type=track";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.AccessToken);

            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<dynamic>(responseContent);

            if (data.tracks.items.Count > 0)
            {
                return data.tracks.items[0].uri;
            }

            return null;
        }

        public async Task<string> SearchAndAddTrack(string playlistName, string name, string artist)
        {
            this.AccessToken = await this.RefreshAccessToken();

            var uri = await this.SearchTrack(name, artist);
            if (uri != null)
            {
                var playlistId = await this.GetPlaylistIdByName(playlistName);
                return await this.AddTrackToPlaylist(playlistId, uri);
            }
            else
            {
                return $"Track {name} by {artist} not found";
            }
        }
        public async Task<string> AddTrackToPlaylist(string playlistId, string trackUri)
        {
            var endpoint = $"v1/playlists/{playlistId}/tracks";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.AccessToken);
            var content = new StringContent(JsonConvert.SerializeObject(new { uris = new[] { trackUri } }), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);
            if (response.IsSuccessStatusCode)
            {
                return "Song added to playlist";
            }
            else
            {
                return $"Error adding track: {response.StatusCode}";
            }
        }
    }
}