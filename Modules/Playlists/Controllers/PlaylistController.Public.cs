using Microsoft.AspNetCore.Mvc;
using AudioArchive.Shared;

namespace AudioArchive.Modules.Playlists.Controllers
{
  public partial class PlaylistController
  {
    [HttpGet]
    public async Task<IActionResult> GetPlaylists()
    {
      var playlists = await _playlistService.GetPlaylistsAsync();
      return Ok(new
      {
        playlists.Count,
        Data = playlists,
      });
    }

    [HttpGet("{playlistId}")]
    public async Task<IActionResult> GetPlaylist([FromRoute] string playlistId)
    {
      if (!Guid.TryParse(playlistId, out var playlistGuid))
        throw new BadRequestException(
          Message: "Could not parse given string into a valid guid.",
          Target: playlistId
        );

      var playlist = await _playlistService.GetPlaylistAsync(playlistGuid)
        ?? throw new NotFoundException(
          Message: $"Playlist entry was not found.",
          Target: playlistId
        );

      return Ok(playlist);
    }
  }
}
