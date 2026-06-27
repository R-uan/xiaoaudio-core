using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AudioArchive.Modules.Playlists.Requests;
using AudioArchive.Shared;

namespace AudioArchive.Modules.Playlists.Controllers
{
  public partial class PlaylistController
  {
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreatePlaylist([FromBody] CreatePlaylistRequest request)
    {
      var result = await _playlistService.CreatePlaylistAsync(request);
      return Ok(result);
    }

    [HttpDelete("{playlistId}")]
    [Authorize]
    public async Task<IActionResult> DeletePlaylist([FromRoute] string playlistId)
    {
      if (!Guid.TryParse(playlistId, out var playlistGuid))
        throw new BadRequestException(
          Message: "Could not parse given string into a valid guid.",
          Target: playlistId
        );

      await _playlistService.DeletePlaylistAsync(playlistGuid);
      return Ok(new
      {
        Message = $"Playlist successfully deleted.",
        Deleted = new
        {
          Id = playlistGuid,
        }
      });
    }

    [HttpPatch("{playlistId}")]
    [Authorize]
    public async Task<IActionResult> UpdatePlaylist(
        [FromRoute] string playlistId,
        [FromBody] PatchPlaylistRequest request
      )
    {
      if (!Guid.TryParse(playlistId, out var playlistGuid))
        throw new BadRequestException(
          Message: "Could not parse given string into a valid guid.",
          Target: playlistId
        );

      var result = await _playlistService.UpdatePlaylistAsync(playlistGuid, request);
      return Ok(result);
    }
  }
}
