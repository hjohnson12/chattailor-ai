using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatTailorAI.Shared.Services.Tools;

namespace ChatTailorAI.Services.Tools.Executors
{
    public class FunctionToolExecutorService : IToolExecutorService
    {
        private delegate Task<string> FunctionDelegate(Dictionary<string, string> args);
        private readonly Dictionary<string, FunctionDelegate> functionMappings;

        private readonly ISpotifyService _spotifyService;

        public FunctionToolExecutorService(
            ISpotifyService spotifyService)
        {
            _spotifyService = spotifyService;

            functionMappings = new Dictionary<string, FunctionDelegate>();

            InitializeFunctionMappings();
        }

        /// <summary>
        /// Executes a specified tool function with provided arguments.
        /// </summary>
        /// <param name="toolName">The name of the tool function to execute.</param>
        /// <param name="toolArguments">A dictionary of arguments required for the tool function.</param>
        /// <returns>A task that represents the asynchronous operation, containing the result as a string.</returns>
        /// <exception cref="NotSupportedException">Thrown when the specified tool function is not supported.</exception>
        /// <exception cref="Exception">Thrown when an error occurs during the execution of the tool function.</exception>
        public async Task<string> Execute(string toolName, Dictionary<string, string> toolArguments)
        {
            try
            {
                if (functionMappings.TryGetValue(toolName, out var functionToCall))
                {
                    return await functionToCall(toolArguments);
                }
                else
                {
                    throw new NotSupportedException($"Function {toolName} is not supported.");
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Error executing tool ${toolName}", e);
            }
        }

        private void InitializeFunctionMappings()
        {
            functionMappings.Add("get_playlist_songs", GetPlaylistSongs);
            functionMappings.Add("get_my_playlist_names", GetMyPlaylistNames);
            functionMappings.Add("add_song_to_playlist", AddSongToPlaylist);
            functionMappings.Add("search_and_play_song", SearchAndPlaySong);
        }

        private async Task<string> GetPlaylistSongs(Dictionary<string, string> args)
        {
            var playlistName = args["playlistName"].ToString();
            var pid = await _spotifyService.GetPlaylistIdByName(playlistName);
            var tracks = await _spotifyService.GetPlaylistTracks(pid);
            return string.Join(",", tracks);
        }

        private async Task<string> GetMyPlaylistNames(Dictionary<string, string> args)
        {
            // TODO: Fix GetUserPlaylists method not working
            var playlists = await _spotifyService.GetUserPlaylists();
            return string.Join(",", playlists.Items.Select(x => x.Name));
        }

        private async Task<string> AddSongToPlaylist(Dictionary<string, string> args)
        {
            var trackName = args["trackName"].ToString();
            var artistName = args["artistName"].ToString();
            var pname = args["playlistName"].ToString();
            return await _spotifyService.SearchAndAddTrack(pname, trackName, artistName);
        }

        private async Task<string> SearchAndPlaySong(Dictionary<string, string> args)
        {
            var trackName = args["trackName"].ToString();
            var artistName = args["artistName"].ToString();
            return await _spotifyService.SearchAndPlayTrack(trackName, artistName);
        }
    }
}